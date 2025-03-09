using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("RolesPermissionsResources")]
public class RolePermissionResource
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

    [ForeignKey("ResourceId")] 
    public Guid? ResourceId { get; set; }

    public Resource? Resource { get; set; }
}

public static class RolePermissionResourceExtensions
{
    public static RolePermissionResourceDto ToDto(this RolePermissionResource rolePermissionResource)
    {
        return new RolePermissionResourceDto
        {
            RoleId = rolePermissionResource.RoleId,
            RoleName = rolePermissionResource.Role.RoleName,
            RoleDescription = rolePermissionResource.Role.RoleDescription,
            PermissionId = rolePermissionResource.PermissionId,
            PermissionName = rolePermissionResource.Permission.PermissionName,
            PermissionDescription = rolePermissionResource.Permission.PermissionDescription,
            ResourceId = rolePermissionResource.ResourceId,
            ResourceName = rolePermissionResource.Resource?.ResourceName,
            ResourceDescription = rolePermissionResource.Resource?.ResourceDescription
        };
    }
}