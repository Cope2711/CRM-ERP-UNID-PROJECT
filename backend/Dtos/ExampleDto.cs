using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ExampleDto
{
    [Required]
    [GuidNotEmpty]
    public Guid id { get; set; }
    
    [MaxLength(50)]
    public string name { get; set; }
}