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
    public Guid id { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    [Unique]
    public string name { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(255)]
    [Unique]
    public string barcode { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public decimal price { get; set; }
    
    [MinLength(4)]
    [MaxLength(255)]
    public string? description { get; set; }
    
    [NonModificable]
    public bool isActive { get; set; }

    [NonModificable]
    public DateTime? createdDate { get; set; }
    
    [NonModificable]
    public DateTime? updatedDate { get; set; }
    
    [ReferenceInfo("brands", "Brand.brandName")]
    public Guid? brandId { get; set; }
    
    [NonModificable]
    public Brand? Brand { get; set; }
    
    [NonModificable]
    [RelationInfo("suppliers", "suppliers-products", new []{ "id", "Supplier.id", "Supplier.name", "Supplier.email" }, "product.id")]
    public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    
    [NonModificable]
    [RelationInfo("categories", "products-categories", new[] { "id", "Category.id", "Category.name" }, "product.id")]
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}

public static class ProductExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            id = product.id,
            name = product.name,
            barcode = product.barcode,
            price = product.price,
            description = product.description,
            isActive = product.isActive,
            brandId = product.brandId
        };
    }
}