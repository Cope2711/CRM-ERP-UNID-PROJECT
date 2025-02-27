using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsResourcesRepository
{
    Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName, string? resourceName = null);
    
    /// <summary>
    /// Checks if a permission identified by its name, along with an optional resource name, is assigned to the specified role.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="permissionName">The name of the permission.</param>
    /// <param name="resourceName">The optional name of the resource. Can be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains true if the permission (and resource, if provided) is assigned to the role; otherwise, false.
    /// </returns>
    Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);
    
    /// <summary>
    /// Checks if a permission identified by its ID, along with an optional resource ID, is assigned to the specified role.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="permissionId">The unique identifier of the permission.</param>
    /// <param name="resourceId">The optional unique identifier of the resource. Can be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains true if the permission (and resource, if provided) is assigned to the role; otherwise, false.
    /// </returns>
    Task<RolePermissionResource?> GetByRoleIdPermissionIdRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);
    
    /// <summary>
    /// Retrieves the role-permission-resource association for the specified role, permission, and optional resource.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="permissionId">The unique identifier of the permission.</param>
    /// <param name="resourceId">The optional unique identifier of the resource. Can be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the role-permission-resource association if found; otherwise, null.
    /// </returns>
    void Add(RolePermissionResource rolePermissionResource);
    
    /// <summary>
    /// Adds a new role-permission-resource association to the repository.
    /// </summary>
    /// <param name="rolePermissionResource">The role-permission-resource association to add.</param>
    Task SaveChangesAsync();
    
    /// <summary>
    /// Saves all pending changes made to the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    void Remove(RolePermissionResource rolePermissionResource);
    /// <summary>
    /// Removes an existing role-permission-resource association from the repository.
    /// </summary>
    /// <param name="rolePermissionResource">The role-permission-resource association to remove.</param>

}

public class RolesPermissionsResourcesResourcesRepository : IRolesPermissionsResourcesRepository
{
    private readonly AppDbContext _context;

    public RolesPermissionsResourcesResourcesRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName, string? resourceName = null)
    {
        return await this._context.RolesPermissionsResources.AnyAsync(rpr =>
            rpr.RoleId == roleId &&
            rpr.Permission.PermissionName == permissionName &&
            (resourceName == null ? rpr.Resource == null : rpr.Resource.ResourceName == resourceName));
    }

    public async Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null)
    {
        return await this._context.RolesPermissionsResources.AnyAsync(rpr =>
            rpr.RoleId == roleId &&
            rpr.PermissionId == permissionId &&
            (resourceId == null ? rpr.ResourceId == null : rpr.ResourceId == resourceId));
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
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && (resourceId == null ? rp.ResourceId == null : rp.ResourceId == resourceId));
    }
    
    public void Remove(RolePermissionResource rolePermissionResource)
    {
        this._context.RolesPermissionsResources.Remove(rolePermissionResource);
    }
}