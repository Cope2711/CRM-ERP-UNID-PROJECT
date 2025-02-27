using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRoleRepository
{
    void Remove(Role role);
    void Update(Role role);
    Task SaveChangesAsync();
    void Add(Role role);
}

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Role role)
    {
        this._context.Roles.Add(role);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
    
    public void Update(Role role)
    { 
        _context.Roles.Update(role);
    }
    
    public void Remove(Role role)
    {
        _context.Roles.Remove(role);
    }
}