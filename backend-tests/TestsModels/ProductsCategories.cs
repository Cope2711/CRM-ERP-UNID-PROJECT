using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class ProductsCategories
    {
        public static readonly ProductCategory iPhone13Tecnology = new ProductCategory
        {
            ProductCategoryId = Guid.Parse("2ef43998-d00c-45e0-9506-24fc5cdc87cf"),
            ProductId = Models.Products.iPhone13.ProductId,
            CategoryId = Models.Categories.Technology.CategoryId,
            CreatedDate = DateTime.UtcNow
        };
    }
}