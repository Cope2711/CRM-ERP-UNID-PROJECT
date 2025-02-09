using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsRepository
{
    Task<RolePermission?> GetByRoleIdAndPermissionIdAsync(Guid roleId, Guid permissionId);
    Task<bool> IsPermissionAssignedToRoleAsync(Guid roleId, Guid permissionId);
    Task Add(RolePermission rolePermission);
    Task SaveChangesAsync();
    void Remove(RolePermission rolePermission);
}

public class RolesPermissionsRepository : IRolesPermissionsRepository
{
    private readonly AppDbContext _context;

    public RolesPermissionsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsPermissionAssignedToRoleAsync(Guid roleId, Guid permissionId)
    {
        return await this._context.RolePermissions.AnyAsync(
            rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
    }

    public async Task Add(RolePermission rolePermission)
    {
        this._context.RolePermissions.Add(rolePermission);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }

    public async Task<RolePermission?> GetByRoleIdAndPermissionIdAsync(Guid roleId, Guid permissionId)
    {
        return await this._context.RolePermissions.Include(rp => rp.Role).Include(rp => rp.Permission).FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
    }
    
    public void Remove(RolePermission rolePermission)
    {
        this._context.RolePermissions.Remove(rolePermission);
    }
}