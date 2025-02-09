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

public class UsersRolesRepository : IUsersRolesRepository
{
    private readonly AppDbContext _context;

    public UsersRolesRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserRole?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId)
    {
        return await this._context.UsersRoles.Include(ur => ur.User).Include(ur => ur.Role).FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
    
    public void Add(UserRole userRole)
    {
        this._context.UsersRoles.Add(userRole);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
    
    public async Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId)
    {
        return await this._context.UsersRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
    
    public void Remove(UserRole userRole)
    {
        _context.UsersRoles.Remove(userRole);
    }
}