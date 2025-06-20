using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Dtos;

public class RequestPasswordResetDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string email { get; set; }

}

public class ResetPasswordDto
{
    [Required(ErrorMessage = "El token es obligatorio.")]
    public string token { get; set; }
    
    [Required(ErrorMessage = "The email is required.")]
    [EmailAddress(ErrorMessage = "The email is not valid.")]
    public string email { get; set; }

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string newPassword { get; set; }

    [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
    public string confirmPassword { get; set; }
}
