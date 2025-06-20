using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Inventory")]
public class Inventory
{
    [Key]
    [NonModificable]
    public Guid id { get; set; }
    
    [Required]
    [ReferenceInfo("products", "Product.name")]
    public Guid productId { get; set; }
    
    [Required]
    [ReferenceInfo("branches", "Branch.name")]
    public Guid branchId { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int quantity { get; set; }
    
    [NonModificable]
    public bool isActive { get; set; }
    
    [NonModificable]
    public DateTime? createdDate { get; set; }
    
    [NonModificable]
    public DateTime? updatedDate { get; set; }
    
    [NonModificable]
    public Product? Product { get; set; }
    
    [NonModificable]
    public Branch? Branch { get; set; }
}

public static class InventoryExtensions
{
    public static InventoryDto ToDto(this Inventory inventory)
    {
        return new InventoryDto
        {
            id = inventory.id,
            productId = inventory.productId,
            branchId = inventory.branchId,
            quantity = inventory.quantity,
            isActive = inventory.isActive
        };
    }
}