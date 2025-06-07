using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Products")]
public class Product
{
    [Key]
    [NonModificable]
    public Guid ProductId { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    [Unique]
    public string ProductName { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(255)]
    [Unique]
    public string ProductBarcode { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public decimal ProductPrice { get; set; }
    
    [MinLength(4)]
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    
    [NonModificable]
    public bool IsActive { get; set; }

    [NonModificable]
    public DateTime? CreatedDate { get; set; }
    
    [NonModificable]
    public DateTime? UpdatedDate { get; set; }
    
    [ReferenceInfo("brands", "Brand.brandName")]
    public Guid? BrandId { get; set; }
    
    [NonModificable]
    public Brand? Brand { get; set; }
    
    [NonModificable]
    [RelationInfo("suppliers", "suppliers-products", new []{ "SupplierProductId", "Supplier.SupplierId", "Supplier.SupplierName", "Supplier.SupplierEmail" })]
    public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    
    [NonModificable]
    [RelationInfo("categories", "products-categories", new[] { "ProductCategoryId", "Category.CategoryId", "Category.CategoryName" })]
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}

public static class ProductExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            ProductBarcode = product.ProductBarcode,
            ProductPrice = product.ProductPrice,
            ProductDescription = product.ProductDescription,
            IsActive = product.IsActive,
            BrandId = product.BrandId,
            Categories = product.ProductCategories.Select(pc => new CategoryDto
            {
                CategoryId = pc.CategoryId,
                CategoryName = pc.Category.CategoryName,
                CategoryDescription = pc.Category.CategoryDescription
            }).ToList()
        };
    }

    public static Product ToModel(this CreateProductDto dto)
    {
        return new Product
        {
            ProductName = dto.ProductName,
            ProductBarcode = dto.ProductBarcode,
            ProductPrice = dto.ProductPrice,
            ProductDescription = dto.ProductDescription,
            IsActive = dto.IsActive,
        };
    }
}