using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class SuppliersProducts
    {
        public static readonly SupplierProduct AppleIphone13 = new SupplierProduct
        {
            SupplierProductId = Guid.Parse("853217ea-0769-4e3b-9153-4a20f3ca2f76"),
            SupplierId = Suppliers.Apple.SupplierId,
            ProductId = Products.iPhone13.ProductId,
            SupplyPrice = 100,
            SupplyLeadTime = 1,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
    }
}