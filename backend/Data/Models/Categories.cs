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
    public Guid id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Unique]
    public string name { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? description { get; set; }
    
    [NonModificable]
    [RelationInfo("products", "products-categories", new []{ "id", "Product.id", "Product.name" }, "category.id")]
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}

public static class CategoryExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            id = category.id,
            name = category.name,
            description = category.description
        };
    }
}