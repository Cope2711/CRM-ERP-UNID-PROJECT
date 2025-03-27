using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersBranchesRepository
{
    void Add(SupplierBranch supplierBranch);
    Task SaveChangesAsync();
    Task<bool> IsSupplierAssignedToBranch(Guid supplierId, Guid branchId);
    void Remove(SupplierBranch supplierBranch);
}

public class SuppliersBranchesRepository(
    AppDbContext _context
) : ISuppliersBranchesRepository
{
    public void Add(SupplierBranch supplierBranch)
    {
        _context.SuppliersBranches.Add(supplierBranch);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> IsSupplierAssignedToBranch(Guid supplierId, Guid branchId)
    {
        return await _context.SuppliersBranches.AnyAsync(sb => sb.SupplierId == supplierId && sb.BranchId == branchId);
    }
    
    public void Remove(SupplierBranch supplierBranch)
    {
        _context.SuppliersBranches.Remove(supplierBranch);
    }
}