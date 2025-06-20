using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Resources")]
public class Resource
{
    [Key]
    public Guid id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string name { get; set; }
    
    [MaxLength(255)]
    public string? description { get; set; }
    
    public ICollection<RolePermissionResource> RolesPermissionsResources { get; set; } = new List<RolePermissionResource>();
}

public static class ResourceExtensions
{
    public static ResourceDto ToDto(this Resource resource)
    {
        return new ResourceDto
        {
            id = resource.id,
            name = resource.name,
            description = resource.description
        };
    }
}