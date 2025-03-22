using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesRepository
{
    void Remove(UserRole userRole);
    void Add(UserRole userRole);
    Task SaveChangesAsync();
    Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId);
    Task<UserRole?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId);
}

public class UsersRolesRepository(
    AppDbContext _context
) : IUsersRolesRepository
{
    public async Task<UserRole?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId)
    {
        return await _context.UsersRoles.Include(ur => ur.User).Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    public void Add(UserRole userRole)
    {
        _context.UsersRoles.Add(userRole);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId)
    {
        return await _context.UsersRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    public void Remove(UserRole userRole)
    {
        _context.UsersRoles.Remove(userRole);
    }
}