using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Roles")]
public class Role
{
    [Key] 
    public Guid RoleId { get; set; }

    [Required] 
    [MaxLength(50)] 
    [Unique]
    public required string RoleName { get; set; }
    
    [Required]
    public required double RolePriority { get; set; }
    
    [MaxLength(255)] 
    public string? RoleDescription { get; set; }
    
    public ICollection<RolePermissionResource> RolesPermissionsResources { get; set; } = new List<RolePermissionResource>();
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