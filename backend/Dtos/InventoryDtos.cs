using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class InventoryDto
{
    public Guid InventoryId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public ProductDto? Product { get; set; }
}

public class CreateInventoryDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateInventoryDto
{
    [GuidNotEmpty]
    public required Guid InventoryId { get; set; }
    [GuidNotEmpty]
    public required Guid ProductId { get; set; }
    public int? Quantity { get; set; }
    public bool? IsActive { get; set; }
}