using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class SuppliersBranches
    {
        public static readonly SupplierBranch AppleHermosilloMiguelHidalgo = new SupplierBranch
        {
            SupplierBranchId = Guid.Parse("cd34763b-d8ec-4e2f-b6da-62a98a11ec07"),
            SupplierId = Suppliers.Apple.SupplierId,
            BranchId = Branches.HermosilloMiguelHidalgo.BranchId,
            IsPreferredSupplier = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
       
        public static readonly SupplierBranch ApplePuertoRico = new SupplierBranch
        {
            SupplierBranchId = Guid.Parse("4e998215-4857-4900-be92-f63cd25b5ab1"),
            SupplierId = Suppliers.Apple.SupplierId,
            BranchId = Branches.PuertoRico.BranchId,
            IsPreferredSupplier = false,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
    }
}