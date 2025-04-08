using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CRM_ERP_UNID.Attributes;

public class IsPhoneNumberWithLadaAttribute : ValidationAttribute
{
    private static readonly Regex _phoneRegex = new Regex(
        @"^(\+52\s?)?(\d{2})\s?\d{4}\s?\d{4}$", 
        RegexOptions.Compiled);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success; 

        var phone = value.ToString();
        if (_phoneRegex.IsMatch(phone))
            return ValidationResult.Success;

        return new ValidationResult($"{validationContext.DisplayName} debe ser un número telefónico válido con lada.");
    }
}