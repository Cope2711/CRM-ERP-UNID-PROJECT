using System.Reflection;
using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CRM_ERP_UNID.Helpers;

/// <summary>
/// Helper class to dynamically generate a schema from a DTO type,
/// reflecting its validation attributes and relationships.
/// </summary>
public static class DtoSchemaHelper
{
    private static readonly Dictionary<Type, Action<Attribute, IDictionary<string, object>>> ValidationExtractors =
        new()
        {
            {
                typeof(MaxLengthAttribute), (attr, schema) =>
                {
                    var a = (MaxLengthAttribute)attr;
                    schema["maxLength"] = a.Length;
                }
            },
            {
                typeof(MinLengthAttribute), (attr, schema) =>
                {
                    var a = (MinLengthAttribute)attr;
                    schema["minLength"] = a.Length;
                }
            },
            {
                typeof(RangeAttribute), (attr, schema) =>
                {
                    var a = (RangeAttribute)attr;
                    schema["min"] = a.Minimum;
                    schema["max"] = a.Maximum;
                }
            },
            {
                typeof(KeyAttribute), (attr, schema) =>
                {
                    schema["key"] = true;
                }
            },
            {
                typeof(NonModificable), (attr, schema) =>
                {
                    schema["nonmodificable"] = true;
                }
            },
            {
                typeof(RelationInfoAttribute), (attr, schema) =>
                {
                    var a = (RelationInfoAttribute)attr;
                    schema["model"] = a.RelationModel;
                    schema["controller"] = a.Controller;
                    schema["selects"] = a.Selects;
                    schema["actualModelKey"] = a.ActualModelKey;
                }
            },
            {
                typeof(ReferenceInfoAttribute), (attr, schema) =>
                {
                    var a = (ReferenceInfoAttribute)attr;
                    schema["controller"] = a.Controller;
                    schema["select"] = a.Select;
                }
            },
            {
                typeof(IsPasswordAttribute), (attr, schema) =>
                {
                    var a = (IsPasswordAttribute)attr;
                    schema["isPassword"] = true;
                }
            }
        };

    /// <summary>
    /// Extracts all the mapped properties of the specified type and converts them into a dictionary.
    /// </summary>
    /// <param name="dtoType">The type from which to extract all the properties.</param>
    /// <param name="ignoreRequired">Indicates whether the [Required] attribute should be ignored.</param>
    /// <returns>A dictionary containing the mapped properties.</returns>
    public static object GetDtoSchema(Type dtoType, bool ignoreRequired)
    {
        var schema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

        var properties = dtoType.GetProperties();

        foreach (var property in properties)
        {
            var propertySchema = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
            var attributes = property.GetCustomAttributes();

            // Obtener tipo real y nullable
            Type type = property.PropertyType;
            bool isNullable = false;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type)!;
                isNullable = true;
            }

            propertySchema["type"] = type.Name.ToLower();

            if (!ignoreRequired)
            {
                propertySchema["required"] = attributes.Any(a => a is RequiredAttribute);
            }
            
            if (isNullable)
                propertySchema["nullable"] = true;

            foreach (var attr in attributes)
            {
                if (ValidationExtractors.TryGetValue(attr.GetType(), out var extractor))
                {
                    extractor(attr, propertySchema);
                }
            }

            schema[StringsHelper.ToCamelCase(property.Name)] = propertySchema;
        }

        return schema;
    }
}