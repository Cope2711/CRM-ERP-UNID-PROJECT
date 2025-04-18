using System.Reflection;
using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Helpers;

public static class DtoSchemaHelper
{
    public static object GetDtoSchema(Type dtoType)
    {
        var schema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

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

            propertySchema["type"] = property.PropertyType.Name.ToLower();
            propertySchema["required"] = attributes.Any(a => a is RequiredAttribute);

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

            var specialAttributes = attributes
                .Where(attr => specialAttributeMap.ContainsKey(attr.GetType()))
                .Select(attr => specialAttributeMap[attr.GetType()])
                .ToList();

            // 👇 Agregar relaciones si es lista de DTOs
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var itemType = property.PropertyType.GetGenericArguments()[0];
                if (itemType.Name.EndsWith("Dto"))
                {
                    specialAttributes.Add("IsRelation");

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

            if (specialAttributes.Any())
                propertySchema["specialData"] = specialAttributes;

            schema[property.Name] = propertySchema;
        }

        return schema;
    }

    public static object GetDtoSchema<T>()
    {
        return GetDtoSchema(typeof(T));
    }
}

