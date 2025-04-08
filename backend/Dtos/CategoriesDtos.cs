using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CategoryDto
{
    [GuidNotEmpty]
    public Guid CategoryId { get; set; }
    
    [MaxLength(50)]
    public required string CategoryName { get; set; }
    
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
}

public class CreateCategoryDto
{
    [Required]
    [MaxLength(50)]
    public required string CategoryName { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
}

public class UpdateCategoryDto
{
    [GuidNotEmpty]
    public Guid CategoryId { get; set; }
    
    [MaxLength(50)]
    public string? CategoryName { get; set; }
    
    [MaxLength(255)]
    public string? CategoryDescription { get; set; }
}