using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Helpers;

public static class DtoSchemaHelper
{
    public static object GetDtoSchema<T>()
    {
        var schema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var propertySchema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
            var rangeAttribute = property.GetCustomAttribute<RangeAttribute>();

            propertySchema["type"] = property.PropertyType.Name.ToLower();
            propertySchema["required"] = property.GetCustomAttribute<RequiredAttribute>() != null;

            if (maxLengthAttribute != null)
                propertySchema["maxLength"] = maxLengthAttribute.Length;
            if (rangeAttribute != null)
            {
                propertySchema["min"] = rangeAttribute.Minimum;
                propertySchema["max"] = rangeAttribute.Maximum;
            }

            schema[property.Name] = propertySchema;
        }

        return schema;
    }
}