using System.Security.Claims;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsResourcesService
{
    Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName,
        string? resourceName = null);
    
    /// <summary>
    /// Checks if a permission identified by its name, along with an optional resource name, is assigned to the specified role.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="permissionName">The name of the permission.</param>
    /// <param name="resourceName">The optional name of the resource. Can be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains true if the permission (and resource, if provided) is assigned to the role; otherwise, false.
    /// </returns>
    Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);
    
    /// <summary>
    /// Checks if a permission identified by its ID, along with an optional resource ID, is assigned to the specified role.
    /// </summary>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <param name="permissionId">The unique identifier of the permission.</param>
    /// <param name="resourceId">The optional unique identifier of the resource. Can be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains true if the permission (and resource, if provided) is assigned to the role; otherwise, false.
    /// </returns>
    Task<RolePermissionResource> AssignPermissionToRoleAsync(PermissionResourceAndRoleDto permissionResourceAndRoleDto);
    
    /// <summary>
    /// Assigns a permission (and optionally a resource) to a role.
    /// </summary>
    /// <param name="permissionResourceAndRoleDto">
    /// A DTO containing the permission, resource, and role details.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the created role-permission-resource association.
    /// </returns>
    Task<RolePermissionResource> RevokePermissionToRoleAsync(PermissionResourceAndRoleDto permissionResourceAndRoleDto);
    
    /// <summary>
    /// Revokes a permission (and optionally a resource) from a role.
    /// </summary>
    /// <param name="PermissionResourceAndRoleDto">
    /// A DTO containing the permission, resource, and role details.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the role-permission-resource association that was removed.
    /// </returns>
    Task<GetAllResponseDto<RolePermissionResource>> GetAllAsync(GetAllDto getAllDto);
    /// <summary>
    /// Retrieves all role-permission-resource associations based on the specified filtering and pagination parameters.
    /// </summary>
    /// <param name="getAllDto">A DTO containing filtering and pagination parameters.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a response DTO with the list of role-permission-resource associations along with associated metadata.
    /// </returns>
}

public class RolesPermissionsResourcesService : IRolesPermissionsResourcesService
{
    private readonly IRolesPermissionsResourcesRepository _rolesPermissionsResourcesRepository;
    private readonly IRoleService _roleService;
    private readonly IPermissionService _permissionService;
    private readonly IGenericServie<RolePermissionResource> _genericService;
    private readonly IResourceService _resourceService;
    private readonly ILogger<RolesPermissionsResourcesService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Guid AuthenticatedUserId =>
        Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   Guid.Empty.ToString());

    public RolesPermissionsResourcesService(IRolesPermissionsResourcesRepository rolesPermissionsResourcesRepository,
        IRoleService roleService,
        IPermissionService permissionService, IGenericServie<RolePermissionResource> genericService,
        IResourceService resourceService, ILogger<RolesPermissionsResourcesService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _rolesPermissionsResourcesRepository = rolesPermissionsResourcesRepository;
        _roleService = roleService;
        _permissionService = permissionService;
        _genericService = genericService;
        _resourceService = resourceService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName,
        string? resourceName = null)
    {
        return await _rolesPermissionsResourcesRepository.ArePermissionNameResourceNameAssignedToRoleIdAsync(roleId,
            permissionName, resourceName);
    }

    public async Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId,
        Guid? resourceId = null)
    {
        return await _rolesPermissionsResourcesRepository.ArePermissionIdResourceIdAssignedToRoleIdAsync(roleId,
            permissionId, resourceId);
    }

    public async Task<GetAllResponseDto<RolePermissionResource>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto,
            query =>
                query.Include(rpr => rpr.Role)
                    .Include(rpr => rpr.Permission)
                    .Include(rpr => rpr.Resource));
    }

    public async Task<RolePermissionResource?> GetByRoleIdPermissionIdResourceIdAsync(Guid roleId, Guid permissionId,
        Guid? resourceId = null)
    {
        return await _rolesPermissionsResourcesRepository.GetByRoleIdPermissionIdRoleIdAsync(roleId, permissionId,
            resourceId);
    }

    public async Task<RolePermissionResource> GetByRoleIdPermissionIdResourceIdThrowsNotFoundAsync(Guid roleId,
        Guid permissionId, Guid? resourceId = null)
    {
        RolePermissionResource? rolePermissionResource =
            await GetByRoleIdPermissionIdResourceIdAsync(roleId, permissionId, resourceId);
        if (rolePermissionResource == null)
        {
            throw new NotFoundException("They are not assigned to role.", field: "PermissionId");
        }

        return rolePermissionResource;
    }

    public async Task<RolePermissionResource> RevokePermissionToRoleAsync(
        PermissionResourceAndRoleDto permissionResourceAndRoleDto)
    {
        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested RevokePermissionToRoleAsync for RoleId {TargetRoleId}, PermissionId {TargetPermissionId} and ResourceId {TargetResourceId}",
            AuthenticatedUserId, permissionResourceAndRoleDto.RoleId, permissionResourceAndRoleDto.PermissionId,
            permissionResourceAndRoleDto.ResourceId);

        // Is already assigned?
        RolePermissionResource rolePermissionResource =
            await GetByRoleIdPermissionIdResourceIdThrowsNotFoundAsync(permissionResourceAndRoleDto.RoleId,
                permissionResourceAndRoleDto.PermissionId, permissionResourceAndRoleDto.ResourceId);

        // Remove from database
        _rolesPermissionsResourcesRepository.Remove(rolePermissionResource);
        await _rolesPermissionsResourcesRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested RevokePermissionToRoleAsync for RoleId {TargetRoleId}, PermissionId {TargetPermissionId} and ResourceId {TargetResourceId} and the permission was revoked",
            AuthenticatedUserId, permissionResourceAndRoleDto.RoleId, permissionResourceAndRoleDto.PermissionId,
            permissionResourceAndRoleDto.ResourceId);

        return rolePermissionResource;
    }

    public async Task<RolePermissionResource> AssignPermissionToRoleAsync(
        PermissionResourceAndRoleDto permissionResourceAndRoleDto)
    {
        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested AssignPermissionToRoleAsync for RoleId {TargetRoleId}, PermissionId {TargetPermissionId} and ResourceId {TargetResourceId}",
            AuthenticatedUserId, permissionResourceAndRoleDto.RoleId, permissionResourceAndRoleDto.PermissionId,
            permissionResourceAndRoleDto.ResourceId);

        // Is already assigned?
        if (await ArePermissionIdResourceIdAssignedToRoleIdAsync(permissionResourceAndRoleDto.RoleId,
                permissionResourceAndRoleDto.PermissionId, permissionResourceAndRoleDto.ResourceId))
        {
            _logger.LogError(
                "User with Id {AuthenticatedUserId} requested AssignPermissionToRoleAsync for RoleId {TargetRoleId}, PermissionId {TargetPermissionId} and ResourceId {TargetResourceId} but the permission is already assigned",
                AuthenticatedUserId, permissionResourceAndRoleDto.RoleId, permissionResourceAndRoleDto.PermissionId,
                permissionResourceAndRoleDto.ResourceId);
            
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

        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested AssignPermissionToRoleAsync for RoleId {TargetRoleId}, PermissionId {TargetPermissionId} and ResourceId {TargetResourceId} and the permission was assigned",
            AuthenticatedUserId, permissionResourceAndRoleDto.RoleId, permissionResourceAndRoleDto.PermissionId,
            permissionResourceAndRoleDto.ResourceId);

        return rolePermissionResource;
    }
}