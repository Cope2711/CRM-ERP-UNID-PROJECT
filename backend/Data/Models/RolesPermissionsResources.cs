using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("RolesPermissionsResources")]
public class RolePermissionResource
{
    [Key] 
    public Guid id { get; set; }
    
    [Required]
    [ForeignKey("roleId")]
    public Guid roleId { get; set; }
    public Role Role { get; set; }
    
    [Required]
    [ForeignKey("permissionId")]
    public Guid permissionId { get; set; }
    public Permission Permission { get; set; }

    [ForeignKey("resourceId")] 
    public Guid? resourceId { get; set; }

    public Resource? Resource { get; set; }
}

public static class RolePermissionResourceExtensions
{
    public static RolePermissionResourceDto ToDto(this RolePermissionResource rolePermissionResource)
    {
        return new RolePermissionResourceDto
        {
            roleId = rolePermissionResource.roleId,
            roleName = rolePermissionResource.Role.name,
            roleDescription = rolePermissionResource.Role.description,
            permissionId = rolePermissionResource.permissionId,
            permissionName = rolePermissionResource.Permission.name,
            permissionDescription = rolePermissionResource.Permission.description,
            resourceId = rolePermissionResource.resourceId,
            resourceName = rolePermissionResource.Resource?.name,
            resourceDescription = rolePermissionResource.Resource?.description
        };
    }
}