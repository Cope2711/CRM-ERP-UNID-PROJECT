using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IPermissionRepository
{
    void AddPermissionAsync(Permission permission);
    Task SaveChangesAsync();
}

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public void AddPermissionAsync(Permission permissions)
    {
        this._context.Permissions.Add(permissions);
    }
    public async Task SaveChangesAsync()    
    {
        await this._context.SaveChangesAsync();
    }
}