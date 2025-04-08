using System.Reflection;
using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Helpers;

public static class DtoSchemaHelper
{
    public static object GetDtoSchema<T>()
    {
        var schema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

        var specialAttributeMap = new Dictionary<Type, string>
        {
            { typeof(IsEmailAttribute), nameof(IsEmailAttribute).Replace("Attribute", "") },
            { typeof(IsPhoneNumberWithLadaAttribute), nameof(IsPhoneNumberWithLadaAttribute).Replace("Attribute", "") }
        };

        var properties = typeof(T).GetProperties();

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

            if (specialAttributes.Any())
                propertySchema["specialData"] = specialAttributes;

            schema[property.Name] = propertySchema;
        }

        return schema;
    }
}
