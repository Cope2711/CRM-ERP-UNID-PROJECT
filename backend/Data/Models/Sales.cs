using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Sales")]
public class Sale
{
    [Key]
    public Guid id { get; set; }

    [Required]
    public Guid branchId { get; set; }

    [Required]
    public Guid userId { get; set; }

    public DateTime? saleDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(12, 2)")]
    public decimal totalAmount { get; set; }

    public DateTime? createdDate { get; set; } = DateTime.UtcNow;
    public DateTime? updatedDate { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(userId))]
    public User? User { get; set; }

    [ForeignKey(nameof(branchId))]
    public Branch? Branch { get; set; }

    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}

public static class SaleExtensions
{
    public static SaleDto ToDto(this Sale sale)
    {
        return new SaleDto
        {
            id = sale.id,
            branchId = sale.branchId,
            userId = sale.userId,
            totalAmount = sale.totalAmount,
            saleDate = sale.saleDate,
            createdDate = sale.createdDate,
            updatedDate = sale.updatedDate,
        };
    }
}