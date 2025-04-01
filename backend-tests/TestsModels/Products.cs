using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Products
    {
        public static readonly Product iPhone13 = new Product
        {
            ProductId = Guid.Parse("5b22cc12-191b-4fe9-9878-bbc1575fa8a7"),
            ProductName = "iPhone 13",
            ProductPrice = 999.99m,
            ProductDescription = "Latest iPhone model",
            IsActive = true,
            BrandId = Brands.Apple.BrandId
        };
        
        public static readonly Product MacBookPro = new Product
        {
            ProductId = Guid.Parse("420a7d01-dd70-417f-872f-aa9f1e1df436"),
            ProductName = "MacBook Pro",
            ProductPrice = 1999.99m,
            ProductDescription = "High-performance laptop",
            IsActive = true,
            BrandId = Brands.Apple.BrandId
        };
        
        public static readonly Product iPadPro = new Product
        {
            ProductId = Guid.Parse("f96bf5e4-9579-404e-9350-087b1ef1305e"),
            ProductName = "iPad Pro",
            ProductPrice = 799.99m,
            ProductDescription = "Powerful tablet for work and entertainment",
            IsActive = true,
            BrandId = Brands.Apple.BrandId
        };
        
        public static readonly Product GalaxyS21 = new Product
        {
            ProductId = Guid.Parse("3179499a-621c-4c07-8acb-e7a057cf4753"),
            ProductName = "Galaxy S21",
            ProductPrice = 899.99m,
            ProductDescription = "Samsung flagship phone",
            IsActive = true,
            BrandId = Brands.Samsung.BrandId
        };
        
        public static readonly Product GalaxyTabS7 = new Product
        {
            ProductId = Guid.Parse("b342289c-c165-4b85-ab6d-2b030f68b171"),
            ProductName = "Galaxy Tab S7",
            ProductPrice = 649.99m,
            ProductDescription = "High-end Android tablet",
            IsActive = true,
            BrandId = Brands.Samsung.BrandId
        };
        
        public static readonly Product SamsungQLEDTV = new Product
        {
            ProductId = Guid.Parse("aa93da89-ba99-4b47-823c-26c5332f6600"),
            ProductName = "Samsung QLED TV",
            ProductPrice = 1500.00m,
            ProductDescription = "Smart TV with stunning display",
            IsActive = true,
            BrandId = Brands.Samsung.BrandId
        };
        
        public static readonly Product NikeAirMax270 = new Product
        {
            ProductId = Guid.Parse("de30da34-c216-488e-8b3b-3cba0fb71baa"),
            ProductName = "Nike Air Max 270",
            ProductPrice = 120.00m,
            ProductDescription = "Comfortable running shoes",
            IsActive = true,
            BrandId = Brands.Nike.BrandId
        };
        
        public static readonly Product NikeZoomX = new Product
        {
            ProductId = Guid.Parse("e9ade468-590a-4c0e-bfbe-91ab9dbb6830"),
            ProductName = "Nike ZoomX Vaporfly Next%",
            ProductPrice = 250.00m,
            ProductDescription = "High-performance running shoes",
            IsActive = true,
            BrandId = Brands.Nike.BrandId
        };
        
        public static readonly Product NikeDriFitTShirt = new Product
        {
            ProductId = Guid.Parse("ac99dcd4-b451-416d-bb12-59b706c5db30"),
            ProductName = "Nike Dri-FIT T-shirt",
            ProductPrice = 30.00m,
            ProductDescription = "Breathable athletic shirt",
            IsActive = true,
            BrandId = Brands.Nike.BrandId
        };
    }
}