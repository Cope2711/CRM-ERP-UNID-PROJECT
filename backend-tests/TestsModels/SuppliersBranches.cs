using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class SuppliersBranches
    {
        public static readonly SupplierBranch AppleHermosilloMiguelHidalgo = new SupplierBranch
        {
            id = Guid.Parse("cd34763b-d8ec-4e2f-b6da-62a98a11ec07"),
            supplierId = Suppliers.Apple.id,
            branchId = Branches.HermosilloMiguelHidalgo.id,
            isPreferredSupplier = true,
            createdDate = DateTime.UtcNow,
            updatedDate = DateTime.UtcNow
        };
       
        public static readonly SupplierBranch ApplePuertoRico = new SupplierBranch
        {
            id = Guid.Parse("4e998215-4857-4900-be92-f63cd25b5ab1"),
            supplierId = Suppliers.Apple.id,
            branchId = Branches.PuertoRico.id,
            isPreferredSupplier = false,
            createdDate = DateTime.UtcNow,
            updatedDate = DateTime.UtcNow
        };
    }
}