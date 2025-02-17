using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

public class ValidOperatorAttribute : ValidationAttribute
{
    private static readonly string[] AllowedOperators =
    {
        "==", "!=", ">", "<", ">=", "<=",
        "StartsWith", "EndsWith", "Contains",
        "In", "Any", "All", "Like"
    };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string operatorValue && !AllowedOperators.Contains(operatorValue))
        {
            return new ValidationResult(
                $"Invalid operator '{operatorValue}'. Allowed operators: {string.Join(", ", AllowedOperators)}");
        }

        return ValidationResult.Success;
    }
}