using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ProductDto
{
    [IsObjectKey]
    public Guid ProductId { get; set; }
    
    [MinLength(3)]
    [MaxLength(50)]
    [Required]
    public required string ProductName { get; set; }
    
    [MinLength(1)]
    [MaxLength(255)]
    [Required]
    public required string ProductBarcode { get; set; }
    
    [Required]
    [Range(0.01, 10000)]
    public required decimal ProductPrice { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    
    public bool IsActive { get; set; }
    
    [Required]
    [GuidNotEmpty]
    public required Guid BrandId { get; set; }
    
    [RelationInfo("ProductsCategories", "products-categories", new[] { "ProductCategoryId", "Category.CategoryId", "Category.CategoryName" })]
    public List<CategoryDto> Categories { get; set; } = new();
    
    [RelationInfo("SupplierProducts", "suppliers-products", new[] { "SupplierProductId", "Supplier.SupplierId", "Supplier.SupplierName" })]
    public List<SupplierDto> Suppliers { get; set; } = new();
}

public class CreateProductDto
{
    [MinLength(3)]
    [MaxLength(50)]
    [Required]
    public required string ProductName { get; set; }
    
    [MinLength(1)]
    [MaxLength(255)]
    [Required]
    public required string ProductBarcode { get; set; }
    
    [Required]
    [Range(0.01, 10000)]
    public required decimal ProductPrice { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    
    public bool IsActive { get; set; }
    
    [Required]
    [GuidNotEmpty]
    public required Guid BrandId { get; set; }
}

public class UpdateProductDto
{
    [MinLength(3)]
    [MaxLength(50)]
    public string? ProductName { get; set; }
    
    [MinLength(1)]
    [MaxLength(255)]
    public string? ProductBarcode { get; set; }
    
    [Range(0.01, 10000)]
    public decimal? ProductPrice { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    
    public bool IsActive { get; set; }
}

public class ChangeBrandProductDto
{
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    [GuidNotEmpty]
    public Guid BrandId { get; set; }
}