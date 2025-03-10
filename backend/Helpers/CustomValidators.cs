using System.Reflection;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Helpers;

public class CustomValidators
{
    public static void ValidateModelContainsColumnsNames(List<FilterDto> filters, Type entityType)
    {
        var validProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToList();

        foreach (var filter in filters)
        {
            if (!validProperties.Contains(filter.Column))
            {
                throw new BadRequestException(
                    $"The field must match a valid column: {string.Join(", ", validProperties)}."
                );
            }
        }
    }

    public static void ValidateModelContainsColumnsNames(string columnName, Type entityType)
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
    
    public static void ValidateModelContainsColumnsNames(List<string> columnsNames, Type entityType)
    {
        var validProperties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToList();


        foreach (var column in columnsNames)
        {
            if (!validProperties.Contains(column))
            {
                throw new BadRequestException(
                    $"The field must match a valid column: {string.Join(", ", validProperties)}."
                );
            }
        }
    }
}