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

public class RoleRepository(
    AppDbContext _context
    ) : IRoleRepository
{
    public void Add(Role role)
    { 
        _context.Roles.Add(role);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
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