using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Sales")]
public class Sale
{
    [Key]
    public Guid SaleId { get; set; }

    [Required]
    public Guid BranchId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public DateTime? SaleDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(12, 2)")]
    public decimal TotalAmount { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [ForeignKey(nameof(BranchId))]
    public Branch? Branch { get; set; }

    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}

public static class SaleExtensions
{
    public static SaleDto ToDto(this Sale sale)
    {
        return new SaleDto
        {
            SaleId = sale.SaleId,
            BranchId = sale.BranchId,
            UserId = sale.UserId,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate,
            CreatedDate = sale.CreatedDate,
            UpdatedDate = sale.UpdatedDate,
            SaleDetails = sale.SaleDetails.Select(sd => sd.ToDto()).ToList()
        };
    }
}