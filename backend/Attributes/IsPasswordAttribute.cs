using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

public class IsPasswordAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var password = value.ToString();
        if (password.Length >= 6)
            return ValidationResult.Success;

        return new ValidationResult($"{validationContext.DisplayName} debe tener al menos 6 caracteres.");
    }
}