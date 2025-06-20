using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Suppliers
    {
        public static readonly Supplier Apple = new Supplier
        {
            id = Guid.Parse("87649222-db8d-4025-82e8-8aa68273fef6"),
            name = "Apple",
            contact = "Apple` Contact",
            email = "Apple@email.com",
            phone = "Apple Phone",
            address = "Apple Address",
            isActive = true,
            createdDate = DateTime.Now,
            updatedDate = DateTime.Now
        };
       
        public static readonly Supplier Xataka = new Supplier{
            id = Guid.Parse("63c455be-c3d9-4a9c-bae9-01562430b1a6"),
            name = "Xataka",
            contact = "Xataka` Contact",
            email = "Xataka@email.com",
            phone = "Xataka Phone",
            address = "Xataka Address",
            isActive = true,
            createdDate = DateTime.Now,
            updatedDate = DateTime.Now
        };
        
        public static readonly Supplier CentelInactive = new Supplier{
            id = Guid.Parse("a0f536b1-0ed8-4f4e-b490-b6228746162e"),
            name = "Centel",
            contact = "Centel` Contact",
            email = "Centel@email.com",
            phone = "Centel Phone",
            address = "Centel Address",
            isActive = false,
            createdDate = DateTime.Now,
            updatedDate = DateTime.Now
        };
    }
}