using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IProductsCategoriesRepository
{
    void Add(ProductCategory productsCategories);
    Task SaveChanges();
    void Remove(ProductCategory productsCategories);
    Task<bool> ExistsByProductCategoryIds(Guid productId, Guid categoryId);
}

public class ProductsCategoriesRepository(
    AppDbContext _context
) : IProductsCategoriesRepository
{
    public void Add(ProductCategory productsCategories)
    {
        _context.ProductsCategories.Add(productsCategories);
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }

    public void Remove(ProductCategory productsCategories)
    {
        _context.ProductsCategories.Remove(productsCategories);
    }
    
    public async Task<bool> ExistsByProductCategoryIds(Guid productId, Guid categoryId)
    {
        return await _context.ProductsCategories.AnyAsync(pc => pc.productId == productId && pc.categoryId == categoryId);
    }
}