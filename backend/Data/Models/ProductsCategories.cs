using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("ProductsCategories")]
public class ProductCategory
{
    [Key]
    public Guid ProductCategoryId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [Required]
    public Guid CategoryId { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public Category? Category { get; set; }
    public Product? Product { get; set; }
}

public static class ProductCategoryExtensions
{
    public static ProductCategoryDto ToDto(this ProductCategory productCategory)
    {
        return new ProductCategoryDto
        {
            ProductCategoryId = productCategory.ProductCategoryId,
            ProductId = productCategory.ProductId,
            CategoryId = productCategory.CategoryId,
            CreatedDate = productCategory.CreatedDate,
        };
    }
}