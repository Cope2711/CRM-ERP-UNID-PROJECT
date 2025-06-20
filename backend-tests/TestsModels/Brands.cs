using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Brands
    {
        public static readonly Brand Apple = new Brand
        {
            id = Guid.Parse("c3146b6f-b50f-4b26-8e77-827fc538b7d1"),
            name = "Apple",
            description = "Apple Inc. - Premium electronics",
            isActive = true
        };
        
        public static readonly Brand Samsung = new Brand
        {
            id = Guid.Parse("809fb57d-ff80-496f-88c3-7b50f0d9b55d"),
            name = "Samsung",
            description = "Samsung Electronics - Leading technology company",
            isActive = true
        };
        
        public static readonly Brand Nike = new Brand
        {
            id = Guid.Parse("5b23b9a8-bd17-4b2a-8e61-b9863a8f77b5"),
            name = "Nike",
            description = "Nike Inc. - Sportswear and equipment",
            isActive = false
        };
    }
}