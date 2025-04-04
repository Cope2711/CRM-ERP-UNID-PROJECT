using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Resources
    {
        public static readonly Resource ProductsCategories = new Resource
        {
            ResourceId = Guid.Parse("3c0659bc-7047-49f4-a5a1-2f3d362eb9ee"),
            ResourceName = "ProductsCategories",
            ResourceDescription = "ProductsCategories module"
        };
        
        public static readonly Resource Categories = new Resource
        {
            ResourceId = Guid.Parse("d25cce2a-ead4-418a-815c-b6fa52f4be92"),
            ResourceName = "Categories",
            ResourceDescription = "Categories module"
        };
        
        public static readonly Resource SuppliersBranches = new Resource
        {
            ResourceId = Guid.Parse("36c11364-f3d7-4860-9aa4-292ac05ad950"),
            ResourceName = "SuppliersBranches",
            ResourceDescription = "Suppliers branches module"
        };
        
        public static readonly Resource SuppliersProducts = new Resource
        {
            ResourceId = Guid.Parse("495123b4-e0b6-4787-80a3-1798572f1fa8"),
            ResourceName = "SuppliersProducts",
            ResourceDescription = "Suppliers products module"
        };
        
        public static readonly Resource Suppliers = new Resource
        {
            ResourceId = Guid.Parse("7e2155e0-5dd3-43e1-8f0f-79688cdead6e"),
            ResourceName = "Suppliers",
            ResourceDescription = "Suppliers module"
        };
        
        public static readonly Resource UsersBranches = new Resource
        {
            ResourceId = Guid.Parse("d161ec8c-7c31-4eb4-a331-82ef9e45999e"),
            ResourceName = "UsersBranches",
            ResourceDescription = "Users branches module"
        };
        
        public static readonly Resource Users = new Resource
        {
            ResourceId = Guid.Parse("d161ec8c-7c31-4eb4-a331-82ef9e45903e"),
            ResourceName = "Users",
            ResourceDescription = "Users module"
        };
        
        public static readonly Resource Roles = new Resource
        {
            ResourceId = Guid.Parse("5688d987-b031-4780-af66-1a99f2fa69dd"),
            ResourceName = "Roles",
            ResourceDescription = "Roles module"
        };
        
        public static readonly Resource Permissions = new Resource
        {
            ResourceId = Guid.Parse("09e63ef5-71ca-4dcb-8f69-4997b02c1e6d"),
            ResourceName = "Permissions",
            ResourceDescription = "Permissions module"
        };
        
        public static readonly Resource UsersRoles = new Resource
        {
            ResourceId = Guid.Parse("85fac418-d875-4f3c-8094-c2d614a58f15"),
            ResourceName = "UsersRoles",
            ResourceDescription = "Users roles module"
        };
        
        public static readonly Resource RolesPermissionsResources = new Resource
        {
            ResourceId = Guid.Parse("67f53f8f-1848-4156-8ee9-ec9e02bd5836"),
            ResourceName = "RolesPermissionsResources",
            ResourceDescription = "Roles permissions resources module"
        };
        
        public static readonly Resource ResourcesResource = new Resource
        {
            ResourceId = Guid.Parse("6193cd07-1a2c-4a7e-95e0-00bb27dbf7c3"),
            ResourceName = "Resources",
            ResourceDescription = "Resources module"
        };
        
        public static readonly Resource ProductsResource = new Resource
        {
            ResourceId = Guid.Parse("a1a2c3d4-1a2c-4a7e-95e0-00bb27dbf7f9"),
            ResourceName = "Products",
            ResourceDescription = "Products module"
        };
        
        public static readonly Resource BrandsResource = new Resource
        {
            ResourceId = Guid.Parse("6691c50e-5bad-4cc3-b035-e2d84ea90c7f"),
            ResourceName = "Brands",
            ResourceDescription = "Brands module"
        };
        
        public static readonly Resource InventoryResource = new Resource
        {
            ResourceId = Guid.Parse("b0f8c2e0-f5a1-4a3e-b5e5-c4e8f0f9c7b7"),
            ResourceName = "Inventory",
            ResourceDescription = "Inventory module"
        };
        
        public static readonly Resource BranchesResource = new Resource
        {
            ResourceId = Guid.Parse("55dc724f-a1aa-4d73-a7ed-5bef32b72be9"),
            ResourceName = "Branches",
            ResourceDescription = "Branches module"
        };
    }
}