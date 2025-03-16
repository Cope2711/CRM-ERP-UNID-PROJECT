using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IInventoryRepository
{
    void Add(Inventory inventory);
    Task SaveChangesAsync();
}

public class InventoryRepository(
    AppDbContext _context
) : IInventoryRepository
{
    public void Add(Inventory inventory)
    {
        _context.Inventory.Add(inventory);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}