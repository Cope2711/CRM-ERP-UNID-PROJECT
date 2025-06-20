using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ResourceDto
{
    [GuidNotEmpty]
    public Guid id { get; set; }
    
    [MaxLength(50)]
    public string name { get; set; }
    
    [MaxLength(255)]
    public string? description { get; set; }
}