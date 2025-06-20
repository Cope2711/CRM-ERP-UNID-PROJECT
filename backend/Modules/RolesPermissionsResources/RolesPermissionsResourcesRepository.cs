using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsResourcesRepository
{
    Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName, string? resourceName = null);
    Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);
    Task<RolePermissionResource?> GetByRoleIdPermissionIdRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);
    void Add(RolePermissionResource rolePermissionResource);
    Task SaveChangesAsync();
    void Remove(RolePermissionResource rolePermissionResource);
}

public class RolesPermissionsResourcesRepository : IRolesPermissionsResourcesRepository
{
    private readonly AppDbContext _context;

    public RolesPermissionsResourcesRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName, string? resourceName = null)
    {
        return await this._context.RolesPermissionsResources.AnyAsync(rpr =>
            rpr.roleId == roleId &&
            rpr.Permission.name == permissionName &&
            (resourceName == null ? rpr.Resource == null : rpr.Resource.name == resourceName));
    }

    public async Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null)
    {
        return await this._context.RolesPermissionsResources.AnyAsync(rpr =>
            rpr.roleId == roleId &&
            rpr.permissionId == permissionId &&
            (resourceId == null ? rpr.resourceId == null : rpr.resourceId == resourceId));
    }
    
    public void Add(RolePermissionResource rolePermissionResource)
    {
        this._context.RolesPermissionsResources.Add(rolePermissionResource);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }

    public async Task<RolePermissionResource?> GetByRoleIdPermissionIdRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null)
    {
        return await this._context.RolesPermissionsResources
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .FirstOrDefaultAsync(rp => rp.roleId == roleId && rp.permissionId == permissionId && (resourceId == null ? rp.resourceId == null : rp.resourceId == resourceId));
    }
    
    public void Remove(RolePermissionResource rolePermissionResource)
    {
        this._context.RolesPermissionsResources.Remove(rolePermissionResource);
    }
}