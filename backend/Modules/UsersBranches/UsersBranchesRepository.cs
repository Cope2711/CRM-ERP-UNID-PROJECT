using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersBranchesRepository
{
    void Add(UserBranch usersBranches);
    void Remove(UserBranch usersBranches);
    Task SaveChanges();
    Task<bool> IsUserAssignedToBranch(Guid userId, Guid branchId);
    Task<UserBranch?> GetByUserIdAndBranchId(Guid userId, Guid branchId);
}

public class UsersBranchesRepository(
    AppDbContext _context
) : IUsersBranchesRepository
{
    public async Task<UserBranch?> GetByUserIdAndBranchId(Guid userId, Guid branchId)
    {
        return await _context.UsersBranches.FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BranchId == branchId);
    }
    
    public void Add(UserBranch usersBranches)
    {
        _context.Add(usersBranches);
    }

    public void Remove(UserBranch usersBranches)
    {
        _context.UsersBranches.Remove(usersBranches);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsUserAssignedToBranch(Guid userId, Guid branchId)
    {
        return await _context.UsersBranches.AnyAsync(ub => ub.UserId == userId && ub.BranchId == branchId);
    }
}