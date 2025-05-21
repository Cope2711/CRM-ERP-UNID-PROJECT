using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Inventory")]
public class Inventory
{
    [Key]
    public Guid InventoryId { get; set; }
    
    [ForeignKey(nameof(Product.ProductId))]
    public required Guid ProductId { get; set; }
    
    [ForeignKey(nameof(Branch.BranchId))]
    public required Guid BranchId { get; set; }
    public required int Quantity { get; set; }
    public required bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public Product? Product { get; set; }
    public Branch? Branch { get; set; }
}

public static class InventoryExtensions
{
    public static InventoryDto ToDto(this Inventory inventory)
    {
        return new InventoryDto
        {
            InventoryId = inventory.InventoryId,
            ProductId = inventory.ProductId,
            BranchId = inventory.BranchId,
            Quantity = inventory.Quantity,
            IsActive = inventory.IsActive,
            CreatedDate = inventory.CreatedDate,
            UpdatedDate = inventory.UpdatedDate,
        };
    }
    
    public static Inventory ToModel(this CreateInventoryDto dto)
    {
        return new Inventory
        {
            ProductId = dto.ProductId,
            BranchId = dto.BranchId,
            Quantity = dto.Quantity,
            IsActive = dto.IsActive ?? true,
        };
    }
}