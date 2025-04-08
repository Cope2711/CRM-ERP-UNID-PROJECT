using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CRM_ERP_UNID.Attributes;

public class IsEmailAttribute : ValidationAttribute
{
    private static readonly Regex _emailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var email = value.ToString();
        if (_emailRegex.IsMatch(email))
            return ValidationResult.Success;

        return new ValidationResult($"{validationContext.DisplayName} debe ser un email válido.");
    }
}