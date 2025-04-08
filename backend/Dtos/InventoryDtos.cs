using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class InventoryDto
{
    [GuidNotEmpty]
    public Guid InventoryId { get; set; }
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    [GuidNotEmpty]
    public Guid? BranchId { get; set; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public ProductDto? Product { get; set; }
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
    [GuidNotEmpty]
    public required Guid InventoryId { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? BranchId { get; set; }
    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }
    public bool? IsActive { get; set; }
}