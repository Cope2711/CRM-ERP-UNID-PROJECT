using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CreateRoleDto
{
    [MinLength(3)]
    [MaxLength(50)]
    [Required]
    public required string RoleName { get; set; }
    
    [Range(0, 1000)]
    [Required]
    public required double RolePriority { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
}

public class RoleDto
{
    [IsObjectKey]
    public Guid RoleId { get; set; }
    
    [MinLength(3)]
    [MaxLength(50)]
    [Required]
    public required string RoleName { get; set; }
    
    [Range(0, 1000)]
    [Required]
    public required double RolePriority { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
    
    [RelationInfo("UsersRoles", "users-roles", new[] { "UserRoleId", "User.UserId", "User.UserUserName" })]
    public List<UserDto> Users { get; set; } = new();
    
    [RelationInfo("RolesPermissions", "roles-permissions", new[] { "RolePermissionId", "Permission.PermissionId", "Permission.PermissionName", "Resource.ResourceName" })]
    public List<PermissionDto> Permissions { get; set; } = new();
}

public class UpdateRoleDto
{
    [MinLength(3)]
    [MaxLength(50)]
    public string? RoleName { get; set; }
    
    [Range(0, 1000)]
    public double? RolePriority { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
}