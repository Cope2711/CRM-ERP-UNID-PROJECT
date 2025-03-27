using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("SuppliersBranches")]
public class SupplierBranch
{
    [Key]
    public Guid SupplierBranchId { get; set; }
    
    [ForeignKey("SupplierId")]
    public required Guid SupplierId { get; set; }
    
    [ForeignKey("BranchId")]
    public required Guid BranchId { get; set; }
    
    public bool? IsPreferredSupplier { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime UpdatedDate { get; set; }
    
    public Supplier? Supplier { get; set; }
    
    public Branch? Branch { get; set; }
}

public static class SupplierBranchExtensions
{
    public static SupplierBranchDto ToDto(this SupplierBranch supplierBranch)
    {
        return new SupplierBranchDto
        {
            SupplierBranchId = supplierBranch.SupplierBranchId,
            SupplierId = supplierBranch.SupplierId,
            BranchId = supplierBranch.BranchId,
            IsPreferredSupplier = supplierBranch.IsPreferredSupplier,
            CreatedDate = supplierBranch.CreatedDate,
            UpdatedDate = supplierBranch.UpdatedDate,
            Supplier = supplierBranch.Supplier?.ToDto(),
            Branch = supplierBranch.Branch?.ToDto()
        };
    }
}

