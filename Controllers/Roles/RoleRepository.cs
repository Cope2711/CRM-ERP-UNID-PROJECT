using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers;

    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(Guid id);
        Task<Role> CreateRoleAsync(Role role);
        Task<Role?> GetByName(string roleName);

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

        public async Task<Role> CreateRoleAsync(Role role)
        {

            if (role.RoleId == Guid.Empty)
            {
                role.RoleId = Guid.NewGuid();
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role?> GetByName(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
        public async Task AddRolePermissionAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
        }
        /*public async Task<List<RoleWithPermissionDtos>> GetRolesWithPermissionAsync(Guid permissionId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .Select(rp => new RoleWithPermissionDtos
                {
                    RoleId = rp.Role.RoleId,
                    RoleName = rp.Role.RoleName,
                    PermissionName = rp.Permission.PermissionName
                })
                .ToListAsync();
        }*/

    }


