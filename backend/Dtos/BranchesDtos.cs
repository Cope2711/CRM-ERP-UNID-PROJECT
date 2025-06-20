using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class BranchDto
{
    [Required]
    [IsObjectKey]
    [GuidNotEmpty]
    public Guid id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string name { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string address { get; set; }
    
    [Required]
    [IsPhoneNumberWithLada]
    public string? phone { get; set; }
    
    public bool isActive { get; set; }
}