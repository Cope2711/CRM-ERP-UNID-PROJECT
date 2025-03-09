using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Resources")]
public class Resource
{
    [Key]
    public Guid ResourceId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string ResourceName { get; set; }
    
    [MaxLength(255)]
    public string? ResourceDescription { get; set; }
    
    public ICollection<RolePermissionResource> RolesPermissionsResources { get; set; } = new List<RolePermissionResource>();
}

public static class ResourceExtensions
{
    public static ResourceDto ToDto(this Resource resource)
    {
        return new ResourceDto
        {
            ResourceId = resource.ResourceId,
            ResourceName = resource.ResourceName,
            ResourceDescription = resource.ResourceDescription
        };
    }
}