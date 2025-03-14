using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ResourceDto
{
    [GuidNotEmpty]
    public Guid ResourceId { get; set; }
    
    [MaxLength(50)]
    public string ResourceName { get; set; }
    
    [MaxLength(255)]
    public string? ResourceDescription { get; set; }
}