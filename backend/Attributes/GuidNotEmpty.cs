using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

public class GuidNotEmpty : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("The Guid cannot be null.");
        }

        if (value is Guid guid && guid == Guid.Empty)
        {
            return new ValidationResult("The Guid cannot be empty.");
        }

        return ValidationResult.Success;
    }
}