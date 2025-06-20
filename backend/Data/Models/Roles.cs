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
    public Guid id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)] 
    [Unique]
    public string name { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public double priority { get; set; }
    
    [MinLength(4)]
    [MaxLength(255)] 
    public string? description { get; set; }
    
    [NonModificable]
    [RelationInfo("permissions", "roles-permissions", new []{ "id", "Permission.id", "Permission.name", "Resource.name" }, "role.id")]
    public ICollection<RolePermissionResource> RolesPermissionsResources { get; set; } = new List<RolePermissionResource>();
    
    [NonModificable]
    [RelationInfo("users", "users-roles", new []{ "id", "User.id", "User.userName" }, "role.id")]
    public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();
}

public static class RoleExtensions
{
    public static RoleDto ToDto(this Role role)
    {
        return new RoleDto
        {
            id = role.id,
            name = role.name,
            priority = role.priority,
            description = role.description
        };
    }
}