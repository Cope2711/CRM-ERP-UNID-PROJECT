using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Roles")]
public class Role
{
    [Key] 
    [NonModificable]
    public Guid RoleId { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)] 
    [Unique]
    public string RoleName { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public double RolePriority { get; set; }
    
    [MinLength(4)]
    [MaxLength(255)] 
    public string? RoleDescription { get; set; }
    
    [NonModificable]
    [RelationInfo("permissions", "roles-permissions", new []{ "RolePermissionId", "Permission.PermissionId", "Permission.PermissionName", "Resource.ResourceName" })]
    public ICollection<RolePermissionResource> RolesPermissionsResources { get; set; } = new List<RolePermissionResource>();
    
    [NonModificable]
    [RelationInfo("users", "users-roles", new []{ "UserRoleId", "User.UserId", "User.UserUserName" })]
    public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();
}

public static class RoleExtensions
{
    public static RoleDto ToDto(this Role role)
    {
        return new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            RolePriority = role.RolePriority,
            RoleDescription = role.RoleDescription
        };
    }

    public static Role ToModel(this CreateRoleDto dto)
    {
        return new Role
        {
            RoleName = dto.RoleName,
            RolePriority = dto.RolePriority,
            RoleDescription = dto.RoleDescription,
        };
    }
}