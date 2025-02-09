using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class PermissionResourceDto
{
    [GuidNotEmpty]
    public Guid PermissionResourceId { get; set; }
    
    [GuidNotEmpty]
    public Guid PermissionId { get; set; }
    
    [MaxLength(50)]
    public string PermissionName { get; set; }
    
    [MaxLength(255)]
    public string? PermissionDescription { get; set; }
    
    [GuidNotEmpty]
    public Guid ResourceId { get; set; }
    
    [MaxLength(50)]
    public string ResourceName { get; set; }
}