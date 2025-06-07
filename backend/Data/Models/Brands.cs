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
    public Guid BrandId { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Unique]
    public required string BrandName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? BrandDescription { get; set; }
    
    [Required]
    [NonModificable]
    public bool IsActive { get; set; }
    
    [NonModificable]
    public DateTime? CreatedDate { get; set; }
    
    [NonModificable]
    public DateTime? UpdatedDate { get; set; }
    
    [NonModificable]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public static class BrandExtensions
{
    public static BrandDto ToDto(this Brand brand)
    {
        return new BrandDto
        {
            BrandId = brand.BrandId,
            BrandName = brand.BrandName,
            BrandDescription = brand.BrandDescription,
            IsActive = brand.IsActive
        };
    }

    public static Brand ToModel(this CreateBrandDto dto)
    {
        return new Brand()
        {
            BrandName = dto.BrandName,
            BrandDescription = dto.BrandDescription,
            IsActive = dto.IsActive
        };
    }
}