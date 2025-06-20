using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class SuppliersProducts
    {
        public static readonly SupplierProduct AppleIphone13 = new SupplierProduct
        {
            id = Guid.Parse("853217ea-0769-4e3b-9153-4a20f3ca2f76"),
            supplierId = Suppliers.Apple.id,
            productId = Products.iPhone13.id,
            supplyPrice = 100,
            supplyLeadTime = 1,
            createdDate = DateTime.UtcNow,
            updatedDate = DateTime.UtcNow
        };
    }
}