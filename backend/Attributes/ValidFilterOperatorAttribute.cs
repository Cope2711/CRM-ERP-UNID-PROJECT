using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Constants;

namespace CRM_ERP_UNID.Attributes;

public class ValidOperatorAttribute : ValidationAttribute
{
    private static readonly string[] AllowedOperators =
    {
        FilterOperators.Equal, FilterOperators.NotEqual, FilterOperators.GreaterThan,
        FilterOperators.LessThan, FilterOperators.GreaterThanOrEqual, FilterOperators.LessThanOrEqual,
        FilterOperators.Like, FilterOperators.Contains, FilterOperators.StartsWith,
        FilterOperators.EndsWith, FilterOperators.In
    };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string operatorValue && !AllowedOperators.Contains(operatorValue))
        {
            return new ValidationResult($"Invalid operator '{operatorValue}'. Allowed operators: {string.Join(", ", AllowedOperators)}");
        }

        return ValidationResult.Success;
    }
}
