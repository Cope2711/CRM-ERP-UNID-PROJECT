using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CategoryDto
{
    [IsObjectKey]
    [GuidNotEmpty]
    [Required]
    public Guid id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string name { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? description { get; set; }
}