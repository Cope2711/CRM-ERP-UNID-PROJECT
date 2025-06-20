using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("SalesDetails")]
public class SaleDetail
{
    [Key]
    public Guid id { get; set; }

    [Required]
    public Guid saleId { get; set; }

    [Required]
    public Guid productId { get; set; }

    [Required]
    public int quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal unitPrice { get; set; }

    [ForeignKey(nameof(saleId))]
    public Sale? Sale { get; set; }

    [ForeignKey(nameof(productId))]
    public Product? Product { get; set; }
}

public static class SaleDetailExtensions
{
    public static SaleDetailDto ToDto(this SaleDetail saleDetail)
    {
        return new SaleDetailDto
        {
            id = saleDetail.id,
            saleId = saleDetail.saleId,
            productId = saleDetail.productId,
            quantity = saleDetail.quantity,
            unitPrice = saleDetail.unitPrice,
            productName = saleDetail.Product?.name ?? string.Empty
        };
    }
}