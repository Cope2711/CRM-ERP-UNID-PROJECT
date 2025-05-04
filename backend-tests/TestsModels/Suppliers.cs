using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Suppliers
    {
        public static readonly Supplier Apple = new Supplier
        {
            SupplierId = Guid.Parse("87649222-db8d-4025-82e8-8aa68273fef6"),
            SupplierName = "Apple",
            SupplierContact = "Apple` Contact",
            SupplierEmail = "Apple@email.com",
            SupplierPhone = "Apple Phone",
            SupplierAddress = "Apple Address",
            IsActive = true,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
       
        public static readonly Supplier Xataka = new Supplier{
            SupplierId = Guid.Parse("63c455be-c3d9-4a9c-bae9-01562430b1a6"),
            SupplierName = "Xataka",
            SupplierContact = "Xataka` Contact",
            SupplierEmail = "Xataka@email.com",
            SupplierPhone = "Xataka Phone",
            SupplierAddress = "Xataka Address",
            IsActive = true,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
        
        public static readonly Supplier CentelInactive = new Supplier{
            SupplierId = Guid.Parse("a0f536b1-0ed8-4f4e-b490-b6228746162e"),
            SupplierName = "Centel",
            SupplierContact = "Centel` Contact",
            SupplierEmail = "Centel@email.com",
            SupplierPhone = "Centel Phone",
            SupplierAddress = "Centel Address",
            IsActive = false,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
    }
}