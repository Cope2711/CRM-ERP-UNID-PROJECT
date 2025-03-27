using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Products")]
public class Product
{
    [Key]
    public Guid ProductId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string ProductName { get; set; }
    
    [Required]
    public decimal ProductPrice { get; set; }
    
    [MaxLength(255)]
    public string? ProductDescription { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    [Required]
    public Guid BrandId { get; set; }
    
    public Brand? Brand { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
}

public static class ProductExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            ProductPrice = product.ProductPrice,
            ProductDescription = product.ProductDescription,
            IsActive = product.IsActive,
            BrandId = product.BrandId
        };
    }
}