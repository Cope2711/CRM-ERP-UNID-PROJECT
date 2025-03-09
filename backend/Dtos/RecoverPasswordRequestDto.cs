using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Dtos;

public class RequestPasswordResetDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; }

}

public class ResetPasswordDto
{
    [Required(ErrorMessage = "El token es obligatorio.")]
    public string Token { get; set; }

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
    public string ConfirmPassword { get; set; }
}
