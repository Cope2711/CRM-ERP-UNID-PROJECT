using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsResourcesService
{
    Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName, string? resourceName = null);
    Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);
    Task<RolePermissionResource> AssignPermissionToRoleAsync(PermissionResourceAndRoleDto permissionResourceAndRoleDto);
    Task<RolePermissionResource> RevokePermissionToRoleAsync(PermissionResourceAndRoleDto permissionResourceAndRoleDto);
    Task<GetAllResponseDto<RolePermissionResource>> GetAllAsync(GetAllDto getAllDto);
}

public class RolesPermissionsResourcesService : IRolesPermissionsResourcesService
{
    private readonly IRolesPermissionsResourcesRepository _rolesPermissionsResourcesRepository;
    private readonly IRoleService _roleService;
    private readonly IPermissionService _permissionService;
    private readonly IGenericServie<RolePermissionResource> _genericService;
    private readonly IResourceService _resourceService;

    public RolesPermissionsResourcesService(IRolesPermissionsResourcesRepository rolesPermissionsResourcesRepository, IRoleService roleService, 
                                            IPermissionService permissionService, IGenericServie<RolePermissionResource> genericService, IResourceService resourceService)      
    {
        _rolesPermissionsResourcesRepository = rolesPermissionsResourcesRepository;
        _roleService = roleService;
        _permissionService = permissionService;
        _genericService = genericService;
        _resourceService = resourceService;
    }
    
    public async Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName, string? resourceName = null)
    {
        return await _rolesPermissionsResourcesRepository.ArePermissionNameResourceNameAssignedToRoleIdAsync(roleId, permissionName, resourceName);
    }

    public async Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId,
        Guid? resourceId = null)
    {
        return await _rolesPermissionsResourcesRepository.ArePermissionIdResourceIdAssignedToRoleIdAsync(roleId, permissionId, resourceId);
    }
    
    public async Task<GetAllResponseDto<RolePermissionResource>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto, 
            query => 
                query.Include(rpr => rpr.Role)
                .Include(rpr => rpr.Permission)
                .Include(rpr => rpr.Resource));
    }
    
    public async Task<RolePermissionResource?> GetByRoleIdPermissionIdResourceIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null)
    {
       return await _rolesPermissionsResourcesRepository.GetByRoleIdPermissionIdRoleIdAsync(roleId, permissionId, resourceId);
    }

    public async Task<RolePermissionResource> GetByRoleIdPermissionIdResourceIdThrowsNotFoundAsync(Guid roleId, Guid permissionId, Guid? resourceId = null)
    {
        RolePermissionResource? rolePermissionResource = await GetByRoleIdPermissionIdResourceIdAsync(roleId, permissionId, resourceId);
        if (rolePermissionResource == null)
        {
            throw new NotFoundException("They are not assigned to role.", field: "PermissionId");
        }
        
        return rolePermissionResource;
    }
    
    public async Task<RolePermissionResource> RevokePermissionToRoleAsync(PermissionResourceAndRoleDto permissionResourceAndRoleDto)
    {
        // Is already assigned?
        RolePermissionResource rolePermissionResource = await GetByRoleIdPermissionIdResourceIdThrowsNotFoundAsync(permissionResourceAndRoleDto.RoleId, permissionResourceAndRoleDto.PermissionId, permissionResourceAndRoleDto.ResourceId);
        
        // Remove from database
        _rolesPermissionsResourcesRepository.Remove(rolePermissionResource);
        await _rolesPermissionsResourcesRepository.SaveChangesAsync();
        
        return rolePermissionResource;
    }

    public async Task<RolePermissionResource> AssignPermissionToRoleAsync(PermissionResourceAndRoleDto permissionResourceAndRoleDto)
    {
        // Is already assigned?
        if (await ArePermissionIdResourceIdAssignedToRoleIdAsync(permissionResourceAndRoleDto.RoleId, permissionResourceAndRoleDto.PermissionId, permissionResourceAndRoleDto.ResourceId))
        {
            throw new UniqueConstraintViolationException("This permission is already assigned to the role.",
                field: "PermissionId");
        }
        
        // Exist source and target?
        await _roleService.GetByIdThrowsNotFoundAsync(permissionResourceAndRoleDto.RoleId);
        await _permissionService.GetByIdThrowsNotFoundAsync(permissionResourceAndRoleDto.PermissionId);
        if (permissionResourceAndRoleDto.ResourceId != null)
            await _resourceService.GetByIdThrowsNotFoundAsync(permissionResourceAndRoleDto.ResourceId.Value);

        // Add to database
        RolePermissionResource rolePermissionResource = new RolePermissionResource
        {
            RoleId = permissionResourceAndRoleDto.RoleId,
            PermissionId = permissionResourceAndRoleDto.PermissionId,
            ResourceId = permissionResourceAndRoleDto.ResourceId
        };
        
        _rolesPermissionsResourcesRepository.Add(rolePermissionResource);
        await _rolesPermissionsResourcesRepository.SaveChangesAsync();
        
        return rolePermissionResource;
    }
}