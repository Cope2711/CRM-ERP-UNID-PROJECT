using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Brands
    {
        public static readonly Brand Apple = new Brand
        {
            BrandId = Guid.Parse("c3146b6f-b50f-4b26-8e77-827fc538b7d1"),
            BrandName = "Apple",
            BrandDescription = "Apple Inc. - Premium electronics",
            IsActive = true
        };
        
        public static readonly Brand Samsung = new Brand
        {
            BrandId = Guid.Parse("809fb57d-ff80-496f-88c3-7b50f0d9b55d"),
            BrandName = "Samsung",
            BrandDescription = "Samsung Electronics - Leading technology company",
            IsActive = true
        };
        
        public static readonly Brand Nike = new Brand
        {
            BrandId = Guid.Parse("5b23b9a8-bd17-4b2a-8e61-b9863a8f77b5"),
            BrandName = "Nike",
            BrandDescription = "Nike Inc. - Sportswear and equipment",
            IsActive = true
        };
    }
}