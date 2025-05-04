using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class BrandDto
{
    [IsObjectKey]
    [GuidNotEmpty]
    [Required]
    public Guid BrandId { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string BrandName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? BrandDescription { get; set; }
    public bool IsActive { get; set; }
}

public class CreateBrandDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string BrandName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? BrandDescription { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateBrandDto
{
    [MinLength(3)]
    [MaxLength(255)]
    public string? BrandName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? BrandDescription { get; set; }
}
