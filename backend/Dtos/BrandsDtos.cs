using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public abstract class BaseBrandDto
{
    [MaxLength(50)]
    public string? BrandName { get; set; }
    
    [MaxLength(255)]
    public string? BrandDescription { get; set; }
    public bool? IsActive { get; set; }
}

public abstract class RequiredBaseBrandDto
{
    [MaxLength(50)]
    public required string BrandName { get; set; }
    
    [MaxLength(255)]
    public string? BrandDescription { get; set; }
    public bool? IsActive { get; set; }
}

public class BrandDto : RequiredBaseBrandDto
{
    [GuidNotEmpty]
    public Guid BrandId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

public class CreateBrandDto : RequiredBaseBrandDto
{
}

public class UpdateBrandDto : BaseBrandDto
{
    [GuidNotEmpty]
    public Guid BrandId { get; set; }
}
