using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("PasswordRecoveryTokens")]
public class PasswordRecoveryToken
{
    [Key]
    public Guid ResetId { get; set; } = Guid.NewGuid();

    // Clave foránea (UserId) que referencia a la tabla Users
    [Required]
    public Guid UserId { get; set; }

    // Token de restablecimiento de contraseña
    [Required]
    public string ResetToken { get; set; }

    // Fecha de expiración del token
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }

    // Relación con la tabla Users (opcional, dependiendo de tu ORM)
    public User User { get; set; }
}