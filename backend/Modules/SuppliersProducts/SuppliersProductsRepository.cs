using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersProductsRepository
{
    void Add(SupplierProduct supplierProduct);
    Task SaveChangesAsync();
    Task<bool> IsProductAssignedToSupplier(Guid supplierId, Guid productId);
    void Remove(SupplierProduct supplierProduct);
}

public class SuppliersProductsRepository(
    AppDbContext _context
) : ISuppliersProductsRepository
{
    public void Add(SupplierProduct supplierProduct)
    {
        _context.SuppliersProducts.Add(supplierProduct);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> IsProductAssignedToSupplier(Guid supplierId, Guid productId)
    {
        return await _context.SuppliersProducts.AnyAsync(sp => sp.supplierId == supplierId && sp.productId == productId);
    }
    
    public void Remove(SupplierProduct supplierProduct)
    {
        _context.SuppliersProducts.Remove(supplierProduct);
    }
}