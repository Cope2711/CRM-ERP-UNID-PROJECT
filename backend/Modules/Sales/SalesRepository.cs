using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM_ERP_UNID.Modules;

public interface ISalesRepository
{
    void Add(Sale sale);
    void Remove(Sale sale);
    Task SaveChanges();
    Task<IDbContextTransaction> BeginTransactionAsync();
}

public class SalesRepository(
    AppDbContext _context
    ) : ISalesRepository
{
    public void Remove(Sale sale)
    {
        _context.Sales.Remove(sale);
    }
    
    public void Add(Sale sale)
    {
        _context.Sales.Add(sale);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}