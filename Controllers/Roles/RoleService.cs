using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Controllers.Permissionss;
using Microsoft.EntityFrameworkCore;
using CRM_ERP_UNID.Data;

namespace CRM_ERP_UNID.Controllers.Roles
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(Guid id);
        Task<Role> CreateRoleAsync(Role role);
        Task<Role> GetByNameThrowsNotFound(string roleName);
        Task<Role> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
        /*Task<List<RoleWithPermissionDtos>> GetRolesWithPermissionAsync(Guid permissionId);*/
    }
        

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly AppDbContext _context;
        
        public RoleService(IRoleRepository roleRepository, IPermissionRepository permissionRepository, AppDbContext context)
        {
            this._roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _context = context;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {id} not found");
            }
            return role;
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            return await _roleRepository.CreateRoleAsync(role);
        }

        public async Task<Role> GetByNameThrowsNotFound(string roleName)
        {
            Role? role = await _roleRepository.GetByName(roleName);

            if (role == null)
            {
                throw new InvalidOperationException($"Role with name: {roleName} not found");
            }
            return role;
        }
       
        public async Task<Role> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);

            if (role == null || permission == null)
            {
                throw new KeyNotFoundException("Role or permission not found.");
            }
            
            //Verifica si ya existe la relación
            var existingRelation = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (existingRelation != null)
            {
                throw new InvalidOperationException("This permission is already assigned to the role.");
            }
            
            //relación entre el rol y el permiso
            _context.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
            await _context.SaveChangesAsync();

            return role;
        }
        
        /*public async Task<List<RoleWithPermissionDtos>> GetRolesWithPermissionAsync(Guid permissionId)
        {
            return await _roleRepository.GetRolesWithPermissionAsync(permissionId);
        }*/
        
    }
}
