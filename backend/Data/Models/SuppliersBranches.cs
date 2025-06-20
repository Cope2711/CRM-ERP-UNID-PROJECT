using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("SuppliersBranches")]
public class SupplierBranch
{
    [Key]
    public Guid id { get; set; }
    
    [ForeignKey("supplierId")]
    public Guid supplierId { get; set; }
    
    [ForeignKey("branchId")]
    public Guid branchId { get; set; }
    
    public bool? isPreferredSupplier { get; set; }
    
    public DateTime createdDate { get; set; }
    
    public DateTime updatedDate { get; set; }
    
    public Supplier? Supplier { get; set; }
    
    public Branch? Branch { get; set; }
}

public static class SupplierBranchExtensions
{
    public static SupplierBranchDto ToDto(this SupplierBranch supplierBranch)
    {
        return new SupplierBranchDto
        {
            id = supplierBranch.id,
            supplierId = supplierBranch.supplierId,
            branchId = supplierBranch.branchId,
            isPreferredSupplier = supplierBranch.isPreferredSupplier,
            createdDate = supplierBranch.createdDate,
            updatedDate = supplierBranch.updatedDate,
            Supplier = supplierBranch.Supplier?.ToDto(),
            Branch = supplierBranch.Branch?.ToDto()
        };
    }
}

