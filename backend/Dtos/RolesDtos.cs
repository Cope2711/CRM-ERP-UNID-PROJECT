using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class RoleDto
{
    [IsObjectKey]
    public Guid id { get; set; }
    
    [MinLength(3)]
    [MaxLength(50)]
    [Required]
    public string name { get; set; }
    
    [Range(0, 1000)]
    [Required]
    public double priority { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? description { get; set; }
}