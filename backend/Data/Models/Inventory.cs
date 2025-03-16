using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Inventory")]
public class Inventory
{
    [Key]
    public Guid InventoryId { get; set; }
    
    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
    
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    
    public Product? Product { get; set; }
}

public static class InventoryExtensions
{
    public static InventoryDto ToDto(this Inventory inventory)
    {
        return new InventoryDto
        {
            InventoryId = inventory.InventoryId,
            ProductId = inventory.ProductId,
            Quantity = inventory.Quantity,
            IsActive = inventory.IsActive,
            CreatedDate = inventory.CreatedDate,
            UpdatedDate = inventory.UpdatedDate,
            Product = inventory.Product?.ToDto()
        };
    }
}