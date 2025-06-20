using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class SupplierDto
{
    [IsObjectKey]
    [GuidNotEmpty]
    [Required]
    public Guid id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string name { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? contact { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    [IsEmail]
    public string? email { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    [IsPhoneNumberWithLada]
    public string? phone { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? address { get; set; }
    
    [Required] 
    public bool isActive { get; set; } = true;
}