using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CreateFullSaleDto
{
    [Required]
    [GuidNotEmpty]
    public Guid BranchId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal TotalAmount { get; set; }
    
    [MinLength(1)]
    [Required]
    public List<CreateSaleDetailDto> SaleDetails { get; set; } = new();
}

public class UpdateSaleDto
{
    [GuidNotEmpty]
    public Guid? BranchId { get; set; }

    [GuidNotEmpty]
    public Guid? UserId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? TotalAmount { get; set; }

    public DateTime? SaleDate { get; set; }
}

public class SaleDto
{
    [Required]
    [GuidNotEmpty]
    [IsObjectKey]
    public Guid SaleId { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid BranchId { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid UserId { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public DateTime? SaleDate { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [RelationInfo("SalesDetails", "sales-details", new[] { "SaleDetailId", "ProductId", "Quantity", "UnitPrice" })]
    public List<SaleDetailDto> SaleDetails { get; set; } = new();
}