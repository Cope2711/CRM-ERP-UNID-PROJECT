using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules.Products;

public interface IProductsRepository
{
    void Add(Product product);
    Task SaveChangesAsync();
}

public class ProductsRepository(
    AppDbContext _context
) : IProductsRepository
{
    public void Add(Product product)
    {
        _context.Products.Add(product);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}