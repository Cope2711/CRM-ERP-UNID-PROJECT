using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IInventoryRepository
{
    void Add(Inventory inventory);
    Task SaveChangesAsync();
    Task<bool> ExistProductInBranch(Guid productId, Guid branchId);
    Task<Inventory?> GetByProductIdInBranchId(Guid productId, Guid branchId);
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
    
    public async Task<bool> ExistProductInBranch(Guid productId, Guid branchId)
    {
        return await _context.Inventory.AnyAsync(i => i.productId == productId && i.branchId == branchId);
    }
    
    public async Task<Inventory?> GetByProductIdInBranchId(Guid productId, Guid branchId)
    {
        return await _context.Inventory.Include(i => i.Product).FirstOrDefaultAsync(i => i.productId == productId && i.branchId == branchId);
    }
}