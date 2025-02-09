using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("UsersRoles")]
public class UserRole
{
    [Key]
    public Guid UserRoleId { get; set; }

    [Required]
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Required]
    [ForeignKey("RoleId")]
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}