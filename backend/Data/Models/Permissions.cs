using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Permissions")]
public class Permission
{
    [Key]
    public Guid id { get; set; }

    [Required]
    [MaxLength(100)]
    public string name { get; set; }

    [MaxLength(255)]
    public string? description { get; set; } 
    
    public ICollection<RolePermissionResource> RolesPermissionsResources { get; set; } = new List<RolePermissionResource>();
}

public static class PermissionExtensions
{
    public static PermissionDto ToDto(this Permission permission)
    {
        return new PermissionDto
        {
            id = permission.id,
            name = permission.name,
            description = permission.description
        };
    }
}

