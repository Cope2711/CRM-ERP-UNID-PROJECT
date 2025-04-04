using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Categories")]
public class Category
{
    [Key]
    public Guid CategoryId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string CategoryName { get; set; }
    
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
    
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
}