using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ProductDto
{
    [IsObjectKey]
    public Guid id { get; set; }
    
    [MinLength(3)]
    [MaxLength(50)]
    [Required]
    public string name { get; set; }
    
    [MinLength(1)]
    [MaxLength(255)]
    [Required]
    public string barcode { get; set; }
    
    [Required]
    [Range(0.01, 10000)]
    public decimal price { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? description { get; set; }
    
    public bool isActive { get; set; }
    
    [GuidNotEmpty]
    [ReferenceInfo("brands", "Brand.brandName")]
    public Guid? brandId { get; set; }
}

public class ChangeBrandProductDto
{
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    [GuidNotEmpty]
    public Guid BrandId { get; set; }
}