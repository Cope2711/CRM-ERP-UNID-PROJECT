using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CreateSaleDetailDto
{
    [Required]
    [GuidNotEmpty]
    public Guid ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}

public class UpdateSaleDetailDto
{
    [GuidNotEmpty]
    public Guid? SaleId { get; set; }

    [GuidNotEmpty]
    public Guid? ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? UnitPrice { get; set; }
}

public class SaleDetailDto
{
    [Required]
    [GuidNotEmpty]
    [IsObjectKey]
    public Guid SaleDetailId { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid SaleId { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }
    
    [Required]
    public required string ProductName { get; set; }
}