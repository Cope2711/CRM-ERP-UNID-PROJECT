using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IBranchesRepository
{
    void Add(Branch branch);
    Task SaveChangesAsync();
}

public class BranchesRepository(
    AppDbContext _context
) : IBranchesRepository
{
    public void Add(Branch branch)
    {
        _context.Branches.Add(branch);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}