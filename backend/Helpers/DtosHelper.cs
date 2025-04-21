using System.Reflection;
using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Helpers;

/// <summary>
/// Helper class to dynamically generate a schema from a DTO type,
/// reflecting its validation attributes and relationships.
/// </summary>
public static class DtoSchemaHelper
{
    /// <summary>
    /// Generates a schema from a given DTO type.
    /// The schema includes type information, validation rules,
    /// nullable info, and special attributes such as custom relationships or formats.
    /// </summary>
    /// <param name="dtoType">The DTO <see cref="Type"/> to inspect.</param>
    /// <returns>An <see cref="ExpandoObject"/> representing the DTO schema.</returns>
    public static object GetDtoSchema(Type dtoType)
    {
        var schema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

        // Maps custom attribute types to short string identifiers
        var specialAttributeMap = new Dictionary<Type, string>
        {
            { typeof(IsEmailAttribute), nameof(IsEmailAttribute).Replace("Attribute", "") },
            { typeof(IsPhoneNumberWithLadaAttribute), nameof(IsPhoneNumberWithLadaAttribute).Replace("Attribute", "") },
            { typeof(IsPasswordAttribute), nameof(IsPasswordAttribute).Replace("Attribute", "") },
            { typeof(IsObjectKeyAttribute), nameof(IsObjectKeyAttribute).Replace("Attribute", "") }
        };

        var properties = dtoType.GetProperties();

        foreach (var property in properties)
        {
            var propertySchema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            var attributes = property.GetCustomAttributes();

            // Extract the underlying type if it's a nullable value type (e.g., decimal?)
            Type type = property.PropertyType;
            bool isNullable = false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type)!;
                isNullable = true;
            }

            // Basic type and required rule
            propertySchema["type"] = type.Name.ToLower();
            propertySchema["required"] = attributes.Any(a => a is RequiredAttribute);

            if (isNullable)
            {
                propertySchema["nullable"] = true;
            }

            // Validation attributes
            var maxLengthAttr = attributes.OfType<MaxLengthAttribute>().FirstOrDefault();
            var minLengthAttr = attributes.OfType<MinLengthAttribute>().FirstOrDefault();
            var rangeAttr = attributes.OfType<RangeAttribute>().FirstOrDefault();

            if (maxLengthAttr != null)
                propertySchema["maxLength"] = maxLengthAttr.Length;

            if (minLengthAttr != null)
                propertySchema["minLength"] = minLengthAttr.Length;

            if (rangeAttr != null)
            {
                propertySchema["min"] = rangeAttr.Minimum;
                propertySchema["max"] = rangeAttr.Maximum;
            }

            // Check for any custom special attributes
            var specialAttributes = attributes
                .Where(attr => specialAttributeMap.ContainsKey(attr.GetType()))
                .Select(attr => specialAttributeMap[attr.GetType()])
                .ToList();

            // Detect one-to-many relationships via List<SomeDto>
            if (property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];
                if (itemType.Name.EndsWith("Dto"))
                {
                    specialAttributes.Add("IsRelation");

                    // Infer relationship model and controller names
                    string mainDtoName = dtoType.Name.Replace("Dto", "");
                    string relatedDtoName = itemType.Name.Replace("Dto", "");
                    string inferredModel = $"{mainDtoName}s{relatedDtoName}s";

                    var relAttr = property.GetCustomAttribute<RelationInfoAttribute>();
                    string model = relAttr?.RelationModel ?? inferredModel;
                    string controller = relAttr?.Controller ?? model.ToLower();
                    string[] selects = relAttr?.Selects ?? Array.Empty<string>();

                    propertySchema["relationInfo"] = new
                    {
                        model,
                        controller,
                        selects
                    };
                }
            }

            // Add special data if any
            if (specialAttributes.Any())
                propertySchema["specialData"] = specialAttributes;

            schema[property.Name] = propertySchema;
        }

        return schema;
    }

    /// <summary>
    /// Generic version of <see cref="GetDtoSchema(Type)"/> that infers the type.
    /// </summary>
    /// <typeparam name="T">The DTO type.</typeparam>
    /// <returns>The generated schema for the DTO type.</returns>
    public static object GetDtoSchema<T>()
    {
        return GetDtoSchema(typeof(T));
    }
}