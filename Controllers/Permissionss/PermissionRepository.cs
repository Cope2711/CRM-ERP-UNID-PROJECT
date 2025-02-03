using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers;

public interface IPermissionRepository
{
    Task<List<Permission>> GetAllPermissionsAsync();
    Task<Permission> GetPermissionByIdAsync(Guid id);
    Task<Permission> CreatePermissionAsync(Permission permission);
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

    public async Task<Permission> CreatePermissionAsync(Permission permissions)
    {
        _context.Permissions.Add(permissions);
        await _context.SaveChangesAsync();
        return permissions;
    }
}