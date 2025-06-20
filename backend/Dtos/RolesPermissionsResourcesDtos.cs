using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public abstract class BaseRolePermissionResourceDto
{
    [GuidNotEmpty(ErrorMessage = "The role id cannot be empty.")]
    public Guid roleId { get; set; }
    
    [MaxLength(50)]
    public string? roleName { get; set; }
    
    [MaxLength(255)]
    public string? roleDescription { get; set; }
    
    [GuidNotEmpty(ErrorMessage = "The permission id cannot be empty.")]
    public Guid permissionId { get; set; }
    
    [MaxLength(100)]
    public string? permissionName { get; set; }
    
    [MaxLength(255)]
    public string? permissionDescription { get; set; }

    [GuidNotEmpty(ErrorMessage = "The resource id cannot be empty.")]
    public Guid? resourceId { get; set; } = null;
    
    [MaxLength(50)]
    public string? resourceName { get; set; }
    
    [MaxLength(255)]
    public string? resourceDescription { get; set; }
}

public class RolePermissionResourceDto : BaseRolePermissionResourceDto { }

public class RolePermissionResourceResponseStatusDto : ResponseStatusDto
{
    public required PermissionResourceAndRoleIdsDto PermissionResourceAndRoleIds { get; set; }
}

public class PermissionsResourcesAndRolesIdsDto
{
    [Required]
    [RangeListLength(1, 50)] 
    public required List<PermissionResourceAndRoleIdsDto> PermissionResourceAndRoleIds { get; set; }
}

public class PermissionResourceAndRoleIdsDto
{
    [GuidNotEmpty]
    public Guid roleId { get; set; }
    
    [GuidNotEmpty]
    public Guid permissionId { get; set; }
    
    public Guid? resourceId { get; set; } = null;
}