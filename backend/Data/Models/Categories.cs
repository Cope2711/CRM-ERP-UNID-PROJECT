using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Categories")]
public class Category
{
    [Key]
    [NonModificable]
    public Guid CategoryId { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Unique]
    public required string CategoryName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
    
    [NonModificable]
    [RelationInfo("products", "products-categories", new []{ "ProductCategoryId", "Product.ProductId", "Product.ProductName" })]
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}

public static class CategoryExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDescription
        };
    }
    
    public static Category ToModel(this CreateCategoryDto dto)
    {
        return new Category
        {
            CategoryName = dto.CategoryName,
            CategoryDescription = dto.CategoryDescription,
        };
    }
}