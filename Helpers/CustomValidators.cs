using System.Reflection;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Helpers;

public class CustomValidators
{
    public static void ValidateModelContainsColumnNameThrowsBadRequest(string? columnName, Type entityType)
    {
        var validProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToList();

        if (!validProperties.Contains(columnName))
        {
            throw new BadRequestException(
                $"The field must match a valid column: {string.Join(", ", validProperties)}."
            );
        }
    }
}