using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class InventoryDto
{
    [IsObjectKey]
    public Guid id { get; set; }
    
    [GuidNotEmpty]
    [ReferenceInfo("products", "Product.ProductName")]
    public Guid productId { get; set; }
    
    [GuidNotEmpty]
    [ReferenceInfo("branches", "Branch.BranchName")]
    public Guid? branchId { get; set; }
    
    [Range(0, int.MaxValue)]
    public int quantity { get; set; }
    public bool isActive { get; set; }
    public DateTime createdDate { get; set; }
    public DateTime updatedDate { get; set; }
}

public class StockChangeDto
{
    public Guid productId { get; set; }
    public int quantity { get; set; } 
}