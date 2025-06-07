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
    public Guid InventoryId { get; set; }
    
    [Required]
    [ReferenceInfo("products", "Product.ProductName")]
    public Guid ProductId { get; set; }
    
    [Required]
    [ReferenceInfo("branches", "Branch.BranchName")]
    public Guid BranchId { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    
    [NonModificable]
    public bool IsActive { get; set; }
    
    [NonModificable]
    public DateTime? CreatedDate { get; set; }
    
    [NonModificable]
    public DateTime? UpdatedDate { get; set; }
    
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
            InventoryId = inventory.InventoryId,
            ProductId = inventory.ProductId,
            BranchId = inventory.BranchId,
            Quantity = inventory.Quantity,
            IsActive = inventory.IsActive
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