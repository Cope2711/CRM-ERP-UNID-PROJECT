using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class UpdateSupplierBranchDto
{
    [GuidNotEmpty]
    public Guid SupplierBranchId { get; set; }
    
    public bool? IsPreferredSupplier { get; set; }
}

public class SupplierBranchDto
{
    [GuidNotEmpty]
    public Guid SupplierBranchId { get; set; }
    
    [GuidNotEmpty]
    public Guid SupplierId { get; set; }
    
    [GuidNotEmpty]
    public Guid BranchId { get; set; }
    
    public bool? IsPreferredSupplier { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public SupplierDto? Supplier { get; set; }
    public BranchDto? Branch { get; set; }
}
