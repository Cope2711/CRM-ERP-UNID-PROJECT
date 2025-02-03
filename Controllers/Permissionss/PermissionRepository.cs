using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers;

public interface IPermissionRepository
{
    Task<List<Permission>> GetAllPermissionsAsync();
    Task<Permission> GetPermissionByIdAsync(Guid id);
    void AddPermissionAsync(Permission permission);
    Task SaveChangesAsync();
    Task<Permission?> GetByName(string permissionName);
}

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        return await _context.Permissions.ToListAsync();
    }

    public async Task<Permission> GetPermissionByIdAsync(Guid id)
    {
        return await _context.Permissions.FindAsync(id);
    }

    public void AddPermissionAsync(Permission permissions)
    {
        this._context.Permissions.Add(permissions);
    }
    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
    public async Task<Permission?> GetByName(string permissionName)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.PermissionName == permissionName);
    }
}