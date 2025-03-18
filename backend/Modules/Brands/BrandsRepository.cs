using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IBrandsRepository
{
    void Add(Brand brand);
    Task SaveChangesAsync();
}

public class BrandsRepository(
    AppDbContext _context
) : IBrandsRepository
{
    public void Add(Brand brand)
    {
        _context.Brands.Add(brand);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}