using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Products
    {
        public static readonly Product iPhone13 = new Product
        {
            id = Guid.Parse("5b22cc12-191b-4fe9-9878-bbc1575fa8a7"),
            name = "iPhone 13",
            barcode = "11111",
            price = 999.99m,
            description = "Latest iPhone model",
            isActive = true,
            brandId = Brands.Apple.id
        };
        
        public static readonly Product MacBookPro = new Product
        {
            id = Guid.Parse("420a7d01-dd70-417f-872f-aa9f1e1df436"),
            name = "MacBook Pro",
            barcode = "2",
            price = 1999.99m,
            description = "High-performance laptop",
            isActive = true,
            brandId = Brands.Apple.id
        };
        
        public static readonly Product iPadPro = new Product
        {
            id = Guid.Parse("f96bf5e4-9579-404e-9350-087b1ef1305e"),
            name = "iPad Pro",
            barcode = "32232",
            price = 799.99m,
            description = "Powerful tablet for work and entertainment",
            isActive = true,
            brandId = Brands.Apple.id
        };
        
        public static readonly Product GalaxyS21 = new Product
        {
            id = Guid.Parse("3179499a-621c-4c07-8acb-e7a057cf4753"),
            name = "Galaxy S21",
            barcode = "4",
            price = 899.99m,
            description = "Samsung flagship phone",
            isActive = true,
            brandId = Brands.Samsung.id
        };
        
        public static readonly Product GalaxyTabS7 = new Product
        {
            id = Guid.Parse("b342289c-c165-4b85-ab6d-2b030f68b171"),
            name = "Galaxy Tab S7",
            barcode = "5",
            price = 649.99m,
            description = "High-end Android tablet",
            isActive = true,
            brandId = Brands.Samsung.id
        };
        
        public static readonly Product SamsungQLEDTV = new Product
        {
            id = Guid.Parse("aa93da89-ba99-4b47-823c-26c5332f6600"),
            name = "Samsung QLED TV",
            barcode = "6",
            price = 1500.00m,
            description = "Smart TV with stunning display",
            isActive = true,
            brandId = Brands.Samsung.id
        };
        
        public static readonly Product NikeAirMax270 = new Product
        {
            id = Guid.Parse("de30da34-c216-488e-8b3b-3cba0fb71baa"),
            name = "Nike Air Max 270",
            barcode = "7",
            price = 120.00m,
            description = "Comfortable running shoes",
            isActive = true,
            brandId = Brands.Nike.id
        };
        
        public static readonly Product NikeZoomX = new Product
        {
            id = Guid.Parse("e9ade468-590a-4c0e-bfbe-91ab9dbb6830"),
            name = "Nike ZoomX Vaporfly Next%",
            barcode = "8",
            price = 250.00m,
            description = "High-performance running shoes",
            isActive = true,
            brandId = Brands.Nike.id
        };
        
        public static readonly Product NikeDriFitTShirt = new Product
        {
            id = Guid.Parse("ac99dcd4-b451-416d-bb12-59b706c5db30"),
            name = "Nike Dri-FIT T-shirt",
            barcode = "9",
            price = 30.00m,
            description = "Breathable athletic shirt",
            isActive = false,
            brandId = Brands.Nike.id
        };
    }
}