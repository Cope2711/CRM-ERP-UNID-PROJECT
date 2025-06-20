using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("ProductsCategories")]
public class ProductCategory
{
    [Key]
    public Guid id { get; set; }
    
    [Required]
    public Guid productId { get; set; }
    
    [Required]
    public Guid categoryId { get; set; }
    
    public DateTime createdDate { get; set; }
    
    public Category? Category { get; set; }
    public Product? Product { get; set; }
}

public static class ProductCategoryExtensions
{
    public static ProductCategoryDto ToDto(this ProductCategory productCategory)
    {
        return new ProductCategoryDto
        {
            id = productCategory.id,
            productId = productCategory.productId,
            categoryId = productCategory.categoryId,
            createdDate = productCategory.createdDate,
        };
    }
}