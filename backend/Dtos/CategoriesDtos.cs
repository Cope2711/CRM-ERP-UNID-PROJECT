using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CategoryDto
{
    [IsObjectKey]
    [GuidNotEmpty]
    [Required]
    public Guid CategoryId { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string CategoryName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
    
    [RelationInfo("ProductsCategories", "products-categories", new[] { "ProductCategoryId", "Product.ProductId", "Product.ProductName" })]
    public List<ProductDto> Products { get; set; } = new();
}

public class CreateCategoryDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string CategoryName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
}

public class UpdateCategoryDto
{
    
    [MinLength(3)]
    [MaxLength(50)]
    public string? CategoryName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
}