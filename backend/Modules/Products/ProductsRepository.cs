using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IProductsRepository
{
    void Add(Product product);
    Task SaveChanges();
}

public class ProductsRepository(
    AppDbContext _context
) : IProductsRepository
{
    public void Add(Product product)
    {
        _context.Products.Add(product);
    }
    
    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}