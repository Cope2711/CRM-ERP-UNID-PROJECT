using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("RolesPermissions")]
public class RolePermission
{
    [Key] 
    public Guid RolePermissionId { get; set; }
    
    [Required]
    [ForeignKey("RoleId")]
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    
    [Required]
    [ForeignKey("PermissionId")]
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }
}