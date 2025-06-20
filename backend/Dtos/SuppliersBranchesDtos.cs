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
    public Guid id { get; set; }
    
    [GuidNotEmpty]
    public Guid supplierId { get; set; }
    
    [GuidNotEmpty]
    public Guid branchId { get; set; }
    
    public bool? isPreferredSupplier { get; set; }
    public DateTime createdDate { get; set; }
    public DateTime updatedDate { get; set; }
    public SupplierDto? Supplier { get; set; }
    public BranchDto? Branch { get; set; }
}
