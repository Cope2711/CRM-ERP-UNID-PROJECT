using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Dtos;

public class PermissionDto
{
    public Guid PermissionId { get; set; }
    public  string PermissionName { get; set; }
    public  string Description { get; set; }
}
public class AssignPermissionDto
{
    [Column("RoleId")]
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}