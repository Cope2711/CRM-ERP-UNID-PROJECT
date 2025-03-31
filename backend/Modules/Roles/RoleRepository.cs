using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesRepository
{
    void Remove(Role role);
    Task SaveChangesAsync();
    void Add(Role role);
    Task<double?> GetRolePriorityById(Guid roleId);
}

public class RolesRepository(
    AppDbContext _context
    ) : IRolesRepository
{
    public async Task<double?> GetRolePriorityById(Guid roleId)
    {
        return await _context.Roles.Where(r => r.RoleId == roleId)
            .Select(r => r.RolePriority)
            .FirstOrDefaultAsync();
    }
    
    public void Add(Role role)
    { 
        _context.Roles.Add(role);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public void Remove(Role role)
    {
        _context.Roles.Remove(role);
    }
}