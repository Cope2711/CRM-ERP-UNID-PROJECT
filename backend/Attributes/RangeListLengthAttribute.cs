using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

public class RangeListLengthAttribute : ValidationAttribute
{
    private readonly int _maxLength;
    private readonly int _minLength;
    
    public RangeListLengthAttribute(int minLength, int maxLength)
    {
        _maxLength = maxLength;
        _minLength = minLength;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IList list)
        {
            if (list.Count < _minLength)
                return new ValidationResult($"The list cannot contain less than {_minLength} items.");
            if (list.Count > _maxLength)
                return new ValidationResult($"The list cannot contain more than {_maxLength} items.");
        }
        return ValidationResult.Success;
    }
}