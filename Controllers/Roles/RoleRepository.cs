using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers;

    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(Guid id);
        Task<Role?> GetByName(string roleName);
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

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.Include(r => r.RolePermissions).ToListAsync();
        }
        
        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            return await _context.Roles.Include(r => r.RolePermissions).FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task<Role?> GetByName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
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