using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsService
{
    Task<bool> IsPermissionAssignedToRoleAsync(Guid roleId, Guid permissionId);
    Task<RolePermission> AssignPermissionToRoleAsync(PermissionAndRoleDto permissionAndRoleDto);
    Task<RolePermission> RevokePermissionToRoleAsync(PermissionAndRoleDto permissionAndRoleDto);
    Task<GetAllResponseDto<RolePermission>> GetAllAsync(GetAllDto getAllDto);
}

public class RolesPermissionsService : IRolesPermissionsService
{
    private readonly IRolesPermissionsRepository _rolesPermissionsRepository;
    private readonly IRoleService _roleService;
    private readonly IPermissionService _permissionService;
    private readonly IGenericServie<RolePermission> _genericService;

    public RolesPermissionsService(IRolesPermissionsRepository rolesPermissionsRepository, IRoleService roleService, IPermissionService permissionService, IGenericServie<RolePermission> genericService)
    {
        _rolesPermissionsRepository = rolesPermissionsRepository;
        _roleService = roleService;
        _permissionService = permissionService;
        _genericService = genericService;
    }

    public async Task<GetAllResponseDto<RolePermission>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto, query => query.Include(rp => rp.Role).Include(rp => rp.Permission));
    }
    
    public async Task<RolePermission?> GetByRoleIdAndPermissionIdAsync(Guid roleId, Guid permissionId)
    {
       return await _rolesPermissionsRepository.GetByRoleIdAndPermissionIdAsync(roleId, permissionId);
    }

    public async Task<RolePermission> GetByRoleIdAndPermissionIdThrowsNotFoundAsync(Guid roleId, Guid permissionId)
    {
        RolePermission? rolePermission = await _rolesPermissionsRepository.GetByRoleIdAndPermissionIdAsync(roleId, permissionId);
        if (rolePermission == null)
        {
            throw new NotFoundException("This permission is not assigned to the role.", field: "PermissionId");
        }
        
        return rolePermission;
    }
    
    public async Task<bool> IsPermissionAssignedToRoleAsync(Guid roleId, Guid permissionId)
    {
        return await _rolesPermissionsRepository.IsPermissionAssignedToRoleAsync(roleId, permissionId);
    }
    
    public async Task<RolePermission> RevokePermissionToRoleAsync(PermissionAndRoleDto permissionAndRoleDto)
    {
        // Is already assigned?
        RolePermission rolePermission = await GetByRoleIdAndPermissionIdThrowsNotFoundAsync(permissionAndRoleDto.RoleId, permissionAndRoleDto.PermissionId);
        
        // Remove from database
        _rolesPermissionsRepository.Remove(rolePermission);
        await _rolesPermissionsRepository.SaveChangesAsync();
        
        return rolePermission;
    }

    public async Task<RolePermission> AssignPermissionToRoleAsync(PermissionAndRoleDto permissionAndRoleDto)
    {
        // Is already assigned?
        if (await IsPermissionAssignedToRoleAsync(permissionAndRoleDto.RoleId, permissionAndRoleDto.PermissionId))
        {
            throw new UniqueConstraintViolationException("This permission is already assigned to the role.",
                field: "PermissionId");
        }
        
        // Exist source and target?
        await _roleService.GetByIdThrowsNotFoundAsync(permissionAndRoleDto.RoleId);
        await _permissionService.GetByIdThrowsNotFoundAsync(permissionAndRoleDto.PermissionId);

        // Add to database
        RolePermission rolePermission = new RolePermission
        {
            RoleId = permissionAndRoleDto.RoleId,
            PermissionId = permissionAndRoleDto.PermissionId
        };
        
        await _rolesPermissionsRepository.Add(rolePermission);
        await _rolesPermissionsRepository.SaveChangesAsync();
        
        return rolePermission;
    }
}