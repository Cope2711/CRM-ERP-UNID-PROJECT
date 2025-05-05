using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Brands")]
public class Brand
{
    [Key]
    public Guid BrandId { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Unique]
    public required string BrandName { get; set; }
    
    [MaxLength(255)]
    public string? BrandDescription { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
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