using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public abstract class BaseProductDto
{
    [MaxLength(50)]
    public string? ProductName { get; set; }
    [Range(0.01, 10000)]
    public decimal? ProductPrice { get; set; }
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    public bool? IsActive { get; set; }
}

public abstract class RequiredBaseProductDto
{
    [Required]
    [MaxLength(50)]
    public required string ProductName { get; set; }
    
    [Required]
    [Range(0.01, 10000)]
    public required decimal ProductPrice { get; set; }
    
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    
    public bool? IsActive { get; set; }
    
    [Required]
    [GuidNotEmpty]
    public required Guid BrandId { get; set; }
}

public class ProductDto : RequiredBaseProductDto
{
    public Guid ProductId { get; set; }
    public bool IsActive { get; set; } 
    
    public List<CategoryDto> Categories { get; set; } = new();
}

public class CreateProductDto : RequiredBaseProductDto
{
}

public class UpdateProductDto : BaseProductDto
{
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
}

public class ChangeBrandProductDto
{
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    [GuidNotEmpty]
    public Guid BrandId { get; set; }
}