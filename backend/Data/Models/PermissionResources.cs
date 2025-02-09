using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("PermissionsResources")]
public class PermissionResource
{
    [Key]
    public Guid PermissionResourceId { get; set; }

    [Required]
    [ForeignKey("PermissionId")]
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }

    [Required]
    [ForeignKey("ResourceId")]
    public Guid ResourceId { get; set; }
    public Resource Resource { get; set; }
}