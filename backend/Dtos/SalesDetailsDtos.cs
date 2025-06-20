using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CreateSaleDetailDto
{
    [Required]
    [GuidNotEmpty]
    public Guid productId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal unitPrice { get; set; }
}

public class SaleDetailDto
{
    [Required]
    [GuidNotEmpty]
    [IsObjectKey]
    public Guid id { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid saleId { get; set; }

    [Required]
    [GuidNotEmpty]
    public Guid productId { get; set; }

    [Required]
    public int quantity { get; set; }

    [Required]
    public decimal unitPrice { get; set; }
    
    [Required]
    public string productName { get; set; }
}