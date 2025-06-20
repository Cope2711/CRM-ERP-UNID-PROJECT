using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Brands")]
public class Brand
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
    
    [Required]
    [NonModificable]
    public bool isActive { get; set; }
    
    [NonModificable]
    public DateTime? createdDate { get; set; }
    
    [NonModificable]
    public DateTime? updatedDate { get; set; }
    
    [NonModificable]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public static class BrandExtensions
{
    public static BrandDto ToDto(this Brand brand)
    {
        return new BrandDto
        {
            id = brand.id,
            name = brand.name,
            description = brand.description,
            IsActive = brand.isActive
        };
    }
}