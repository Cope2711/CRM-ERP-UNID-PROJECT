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
    Task<bool> AreUsersInSameBranch(Guid userId1, Guid userId2);
    Task<UserBranch?> GetByUserIdAndBranchId(Guid userId, Guid branchId);
}

public class UsersBranchesRepository(
    AppDbContext _context
) : IUsersBranchesRepository
{
    public async Task<bool> AreUsersInSameBranch(Guid userId1, Guid userId2)
    {
        bool user1HasNoBranches = !await _context.UsersBranches.AnyAsync(ub => ub.UserId == userId1);
        bool user2HasNoBranches = !await _context.UsersBranches.AnyAsync(ub => ub.UserId == userId2);

        if (user1HasNoBranches || user2HasNoBranches)
        {
            return true;
        }

        return await _context.UsersBranches
            .AnyAsync(ub1 => ub1.UserId == userId1 &&
                             _context.UsersBranches.Any(ub2 => ub2.UserId == userId2 &&
                                                               ub2.BranchId == ub1.BranchId));
    }
    
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