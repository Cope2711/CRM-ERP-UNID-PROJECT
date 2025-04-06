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
    Task<double> GetMaxUserRolePriority(Guid userId);
}

public class UsersRolesRepository(
    AppDbContext _context
) : IUsersRolesRepository
{
    public async Task<double> GetMaxUserRolePriority(Guid userId)
    {
        return await _context.UsersRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.RolePriority)
            .MaxAsync();
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