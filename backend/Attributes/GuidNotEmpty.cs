using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

public class GuidNotEmpty : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("The value cannot be null.");
        }

        if (value is Guid guidValue)
        {
            if (guidValue == Guid.Empty)
            {
                return new ValidationResult("The GUID cannot be empty.");
            }

            return ValidationResult.Success;
        }

        if (value is IEnumerable enumerableValue)
        {
            var guids = enumerableValue.Cast<object>()
                .Where(x => x is Guid)
                .Cast<Guid>()
                .ToList();

            if (!guids.Any())
            {
                return new ValidationResult("The list cannot be empty.");
            }

            if (guids.Any(g => g == Guid.Empty))
            {
                return new ValidationResult("The list contains an empty GUID.");
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid type. Expected a GUID or a list of GUIDs.");
    }
}