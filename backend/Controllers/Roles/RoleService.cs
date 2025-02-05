using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Controllers;

    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(Guid id);
        Task<RoleDto> CreateRoleAsync(RoleDto role);
        Task<RoleDto> GetByNameThrowsNotFound(string roleName);
        Task<RoleDto> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        
        
        public RoleService(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return roles.Select(role => new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RolePermissions = role.RolePermissions?.Select(rp => new RolePermissionDto
                {
                    PermissionId = rp.PermissionId
                }).ToList()  
            }).ToList();
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException($"Role with ID {id} not found", field:"Role");
            }
            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                RolePermissions = role.RolePermissions?.Select(rp => new RolePermissionDto
                {
                    PermissionId = rp.PermissionId
                }).ToList()
            };
        }

        public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
        {
            // Verificar si ya existe un rol con el mismo nombre para evitar duplicados
            var existingRole = await _roleRepository.GetByName(roleDto.RoleName);
            if (existingRole != null)
            {
                throw new UniqueConstraintViolationException($"A role with the name '{roleDto.RoleName}' already exists.", field: "RoleName");
            }

            var newRole = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = roleDto.RoleName,
            };

             this._roleRepository.AddRoleAsync(newRole);
            await this._roleRepository.SaveChangesAsync(); 

            return new RoleDto
            {
                RoleId = newRole.RoleId,
                RoleName = newRole.RoleName,
            };
        }

        public async Task<RoleDto> GetByNameThrowsNotFound(string roleName)
        {
            var role = await _roleRepository.GetByName(roleName);
            if (role == null)
            {
                throw new NotFoundException($"Role with name: {roleName} not found", field:"RoleNmae");
            }
            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }
       
        public async Task<RoleDto> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);

            if (role == null || permission == null)
            {
                throw new NotFoundException($"Role with ID {roleId} not found.", field: "RoleId");
            }

            var existingRelation = role.RolePermissions?.FirstOrDefault(rp => rp.PermissionId == permissionId);
            if (existingRelation != null)
            {
                throw new UniqueConstraintViolationException("This permission is already assigned to the role.", field: "PermissionId");
            }

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            await _roleRepository.AddRolePermissionAsync(rolePermission);
            return await GetRoleByIdAsync(roleId);
        }
    }