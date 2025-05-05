using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Products")]
public class Product
{
    [Key]
    public Guid ProductId { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Unique]
    public required string ProductName { get; set; }
    
    [Required]
    [MaxLength(255)]
    [Unique]
    public required string ProductBarcode { get; set; }
    
    [Required]
    public decimal ProductPrice { get; set; }
    
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    public Guid? BrandId { get; set; }
    
    public Brand? Brand { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
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