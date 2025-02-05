using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("RefreshTokens")]
public class RefreshToken
{
    [Key] public Guid RefreshTokenId { get; set; }

    [Required] public Guid UserId { get; set; }

    [Required] [MaxLength(200)] public string Token { get; set; }

    [Required] public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}