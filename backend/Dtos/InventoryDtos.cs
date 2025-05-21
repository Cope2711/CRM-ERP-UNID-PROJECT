using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class InventoryDto
{
    [IsObjectKey]
    public Guid InventoryId { get; set; }
    
    [GuidNotEmpty]
    [ReferenceInfo("products", "Product.ProductName")]
    public Guid ProductId { get; set; }
    
    [GuidNotEmpty]
    [ReferenceInfo("branches", "Branch.BranchName")]
    public Guid? BranchId { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class CreateInventoryDto
{
    [Required]
    [GuidNotEmpty]
    public required Guid ProductId { get; set; }
    
    [Required]
    [GuidNotEmpty]
    public required Guid BranchId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateInventoryDto
{
    public Guid? ProductId { get; set; }
    public Guid? BranchId { get; set; }
    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }
    public bool? IsActive { get; set; }
}

public class StockChangeDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } 
}