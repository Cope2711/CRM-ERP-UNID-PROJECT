using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("PasswordResets")]
public class PasswordReset
{
    [Key]
    public Guid ResetId { get; set; } = Guid.NewGuid();

    // Clave foránea (UserId) que referencia a la tabla Users
    public Guid UserId { get; set; }

    // Token de restablecimiento de contraseña
    public string ResetToken { get; set; }

    // Fecha de expiración del token
    public DateTime ResetTokenExpiry { get; set; }

    // Relación con la tabla Users (opcional, dependiendo de tu ORM)
    public User User { get; set; }
}