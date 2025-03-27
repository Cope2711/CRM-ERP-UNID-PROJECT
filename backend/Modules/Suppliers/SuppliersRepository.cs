using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersRepository
{
    void Add(Supplier supplier);
    Task SaveChanges();
}

public class SuppliersRepository(
    AppDbContext _context
) : ISuppliersRepository
{
    public void Add(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}