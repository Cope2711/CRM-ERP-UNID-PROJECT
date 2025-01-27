using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("RolePermissions")]
public class RolePermission
{
    [Key] public Guid RolePermissionId { get; set; }
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}