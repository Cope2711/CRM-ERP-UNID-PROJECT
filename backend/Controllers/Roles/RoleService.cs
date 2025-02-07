using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Controllers;

public interface IRoleService
{
    Task<GetAllResponseDto<Role>> GetAllAsync(GetAllDto getAllDto);
    Task<Role> GetByIdThrowsNotFoundAsync(Guid id);
    Task<RoleDto> CreateRoleAsync(RoleDto role);
    Task<Role> GetByNameThrowsNotFoundAsync(string roleName);
    Task<Role?> GetByNameAsync(string roleName);
    Task<Role> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
}

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IGenericServie<Role> _genericService;
    private readonly IPermissionService _permissionService;

    public RoleService(IRoleRepository roleRepository, IGenericServie<Role> genericService, IPermissionService permissionService)
    {
        _roleRepository = roleRepository;
        _genericService = genericService;
        _permissionService = permissionService;
    }

    public async Task<GetAllResponseDto<Role>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<Role> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id);
    }


    public async Task<RoleDto> CreateRoleAsync(RoleDto roleDto)
    {
        // Verificar si ya existe un rol con el mismo nombre para evitar duplicados
        Role existingRole = await GetByNameAsync(roleDto.RoleName);
        if (existingRole != null)
        {
            throw new UniqueConstraintViolationException($"A role with the name '{roleDto.RoleName}' already exists.",
                field: "RoleName");
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

    public async Task<Role> GetByNameThrowsNotFoundAsync(string roleName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(r => r.RoleName, roleName);
    }
    
    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _genericService.GetFirstAsync(r => r.RoleName, roleName);
    }

    public async Task<Role> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
    {
        Role role = await _genericService.GetByIdThrowsNotFoundAsync(roleId);
        
        Permission permission = await _permissionService.GetByIdThrowsNotFoundAsync(permissionId);

        if (role == null || permission == null)
        {
            throw new NotFoundException($"Role with ID {roleId} not found.", field: "RoleId");
        }

        var existingRelation = role.RolePermissions?.FirstOrDefault(rp => rp.PermissionId == permissionId);
        if (existingRelation != null)
        {
            throw new UniqueConstraintViolationException("This permission is already assigned to the role.",
                field: "PermissionId");
        }

        var rolePermission = new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };

        await _roleRepository.AddRolePermissionAsync(rolePermission);
        return await GetByIdThrowsNotFoundAsync(roleId);
    }
}