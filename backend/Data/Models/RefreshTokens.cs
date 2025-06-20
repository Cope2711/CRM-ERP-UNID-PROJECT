using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("RefreshTokens")]
public class RefreshToken
{
    [Key] public Guid id { get; set; }

    [Required] public Guid userId { get; set; }

    [Required] [MaxLength(200)] public string token { get; set; }

    [Required] [MaxLength(255)] public string deviceId { get; set; }
    
    [Required] public DateTime expiresAt { get; set; }

    public DateTime? revokedAt { get; set; }

    [ForeignKey(nameof(userId))] public virtual User User { get; set; }
}