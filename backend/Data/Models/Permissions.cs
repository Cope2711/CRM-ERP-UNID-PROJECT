using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Permissions")]
public class Permission
{
    [Key]
    public Guid PermissionId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string PermissionName { get; set; }

    [MaxLength(255)]
    public string? PermissionDescription { get; set; } 
    
    public ICollection<RolePermissionResource> RolesPermissionsResources { get; set; } = new List<RolePermissionResource>();
}

public static class PermissionExtensions
{
    public static PermissionDto ToDto(this Permission permission)
    {
        return new PermissionDto
        {
            PermissionId = permission.PermissionId,
            PermissionName = permission.PermissionName,
            PermissionDescription = permission.PermissionDescription
        };
    }
}

