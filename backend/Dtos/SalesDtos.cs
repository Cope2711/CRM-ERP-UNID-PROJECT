using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CreateFullSaleDto
{
    [Required]
    [GuidNotEmpty]
    public Guid branchId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal totalAmount { get; set; }
    
    [MinLength(1)]
    [Required]
    public List<CreateSaleDetailDto> SaleDetails { get; set; } = new();
}

public class SaleDto
{
    [Required]
    [GuidNotEmpty]
    [IsObjectKey]
    public Guid id { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid branchId { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid userId { get; set; }

    [Required]
    public decimal totalAmount { get; set; }

    [Required]
    public DateTime? saleDate { get; set; }

    public DateTime? createdDate { get; set; }
    public DateTime? updatedDate { get; set; }
}