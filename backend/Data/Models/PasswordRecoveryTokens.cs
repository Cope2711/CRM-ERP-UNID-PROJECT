using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("PasswordRecoveryTokens")]
public class PasswordRecoveryToken
{
    [Key]
    public Guid id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid userId { get; set; }

    [Required]
    public string resetToken { get; set; }

    [Required]
    public DateTime expiresAt { get; set; }
    
    [Required]
    public DateTime createdAt { get; set; }

    public User User { get; set; }
}