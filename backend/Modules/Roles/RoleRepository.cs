using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IRoleRepository
{
    Task SaveChangesAsync();
    void AddRoleAsync(Role role);
    Task AddRolePermissionAsync(RolePermission rolePermission);
}

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddRolePermissionAsync(RolePermission rolePermission)
    {
        _context.RolePermissions.Add(rolePermission);
    }

    public void AddRoleAsync(Role role)
    {
        this._context.Roles.Add(role);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}