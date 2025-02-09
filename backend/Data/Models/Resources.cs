using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("Resources")]
public class Resource
{
    [Key]
    public Guid ResourceId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string ResourceName { get; set; }
    
    public ICollection<PermissionResource> PermissionResources { get; set; } = new List<PermissionResource>();
}