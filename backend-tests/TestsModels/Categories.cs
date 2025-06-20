using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Categories
    {
        public static readonly Category Technology = new Category
        {
            id = Guid.Parse("8652fd98-d41d-4be9-9b51-2c8fe4f24e93"),
            name = "Technology",
            description = "Technology category"
        };
        
        public static readonly Category Men = new Category
        {
            id = Guid.Parse("b28179fb-e9a4-4bb9-92f2-828027f59416"),
            name = "Men",
            description = "Men category"
        };
    }
}