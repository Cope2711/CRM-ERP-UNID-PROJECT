using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CRM_ERP_UNID.Attributes;

public class IsPhoneNumberWithLadaAttribute : ValidationAttribute
{
    private static readonly Regex _phoneRegex = new Regex(
        @"^\+.*$",
        RegexOptions.Compiled);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var phone = value.ToString();

        if (phone.Length == 13 && _phoneRegex.IsMatch(phone))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"{validationContext.DisplayName} debe tener 13 caracteres y comenzar con '+'.");
    }
}