using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

public class GuidNotEmptyAttribute : ValidationAttribute
{
    public GuidNotEmptyAttribute() 
        : base("The value cannot be an empty GUID.")
    { }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is Guid guidValue && guidValue == Guid.Empty)
        {
            return new ValidationResult(ErrorMessage ?? "The GUID cannot be empty.");
        }

        return ValidationResult.Success; 
    }
}