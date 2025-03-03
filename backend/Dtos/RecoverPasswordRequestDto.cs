using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Dtos;

// DTO para solicitar el token
public class RequestPasswordResetDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; }

}

// DTO para reiniciar la contraseña
public class ResetPasswordDto
{
    [Required(ErrorMessage = "El token es obligatorio.")]
    public string Token { get; set; }

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
    [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; }
}
