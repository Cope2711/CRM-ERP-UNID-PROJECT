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

public class SuppliersAndBranchesDto
{
    [RangeListLength(1, int.MaxValue)]
    public required List<SupplerAndBranchIdDto> SupplerAndBranchIdDto { get; set; }
}

public class SupplerAndBranchIdDto
{
    [GuidNotEmpty]
    public Guid SupplierId { get; set; }
    
    [GuidNotEmpty]
    public Guid BranchId { get; set; }
}

public class SuppliersBranchResponseStatusDto : ResponseStatusDto
{
    public SupplerAndBranchIdDto SupplerAndBranchId { get; set; }
}

public class SupplierBranchIdDto
{
    [GuidNotEmpty]
    public Guid SupplierBranchId { get; set; }
}

public class SuppliersBranchesIdsDto
{
    [RangeListLength(1, int.MaxValue)]
    public required List<SupplierBranchIdDto> SupplierBranchIds { get; set; }
}

public class SuppliersBranchesRevokedResponseStatusDto : ResponseStatusDto
{
    public SupplierBranchIdDto SupplierBranchId { get; set; }
}

