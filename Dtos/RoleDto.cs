using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Dtos;

public class RoleDto
{
    [Column("RoleId")]
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public List<RolePermissionDto>? RolePermissions { get; set; }
    
}