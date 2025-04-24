using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("SalesDetails")]
public class SaleDetail
{
    [Key]
    public Guid SaleDetailId { get; set; }

    [Required]
    public Guid SaleId { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal UnitPrice { get; set; }

    [ForeignKey(nameof(SaleId))]
    public Sale? Sale { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
}

public static class SaleDetailExtensions
{
    public static SaleDetailDto ToDto(this SaleDetail saleDetail)
    {
        return new SaleDetailDto
        {
            SaleDetailId = saleDetail.SaleDetailId,
            SaleId = saleDetail.SaleId,
            ProductId = saleDetail.ProductId,
            Quantity = saleDetail.Quantity,
            UnitPrice = saleDetail.UnitPrice,
            ProductName = saleDetail.Product?.ProductName ?? string.Empty
        };
    }
}