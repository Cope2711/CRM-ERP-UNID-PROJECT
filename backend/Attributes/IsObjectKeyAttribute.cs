using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

public class IsObjectKeyAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var objectKey = value.ToString();
        if (objectKey.Length == 0)
            return ValidationResult.Success;

        return new ValidationResult($"{validationContext.DisplayName} debe ser un object key válido.");
    }
}