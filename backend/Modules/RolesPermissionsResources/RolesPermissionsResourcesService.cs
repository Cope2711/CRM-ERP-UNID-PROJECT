using System.Security.Claims;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsResourcesService
{
    Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName,
        string? resourceName = null);

    Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);

    Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> AssignPermissionsToRolesAsync(
        PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto);

    Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> RevokePermissionsToRolesAsync(PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto);

    Task<GetAllResponseDto<RolePermissionResource>> GetAllAsync(GetAllDto getAllDto);
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

    public async Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> RevokePermissionsToRolesAsync(
        PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        ResponsesDto<RolePermissionResourceResponseStatusDto> responsesDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokePermissionToRoleAsync with the object {permissionsResourcesAndRolesIdsDto}",
            authenticatedUserId, permissionsResourcesAndRolesIdsDto);

        foreach (PermissionResourceAndRoleIdsDto permissionResourceAndRoleIdsDto in permissionsResourcesAndRolesIdsDto
            .PermissionResourceAndRoleIds)
        {
            RolePermissionResource? rolePermissionResource =
                await GetByRoleIdPermissionIdResourceIdAsync(permissionResourceAndRoleIdsDto.RoleId,
                    permissionResourceAndRoleIdsDto.PermissionId, permissionResourceAndRoleIdsDto.ResourceId);
            
            if (rolePermissionResource == null)
            {
                responsesDto.Failed.Add(new RolePermissionResourceResponseStatusDto {
                    PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                    Status = ResponseStatus.NotFound,
                    Field = "PermissionId",
                    Message = "Permission not found"
                }); continue;
            }
            
            // Remove from database
            _rolesPermissionsResourcesRepository.Remove(rolePermissionResource);
            await _rolesPermissionsResourcesRepository.SaveChangesAsync();
            
            responsesDto.Success.Add(new RolePermissionResourceResponseStatusDto
            {
                PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                Status = ResponseStatus.Success,
                Message = "Success"
            });
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignPermissionToRoleAsync and the result was {responsesDto}",
            authenticatedUserId, responsesDto);

        return responsesDto;
    }

    public async Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> AssignPermissionsToRolesAsync(
            PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        ResponsesDto<RolePermissionResourceResponseStatusDto> responsesDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignPermissionToRoleAsync with the object {permissionsResourcesAndRolesIdsDto}",
            authenticatedUserId, permissionsResourcesAndRolesIdsDto);

        foreach (PermissionResourceAndRoleIdsDto permissionResourceAndRoleIdsDto in permissionsResourcesAndRolesIdsDto
                     .PermissionResourceAndRoleIds)
        {
            if (await ArePermissionIdResourceIdAssignedToRoleIdAsync(permissionResourceAndRoleIdsDto.RoleId,
                    permissionResourceAndRoleIdsDto.PermissionId, permissionResourceAndRoleIdsDto.ResourceId)) {
                responsesDto.Failed.Add(new RolePermissionResourceResponseStatusDto {
                    PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                    Status = ResponseStatus.AlreadyProcessed,
                    Message = "Already assigned"
                }); continue;
            }

            if (!await _roleService.ExistById(permissionResourceAndRoleIdsDto.RoleId)) {
                responsesDto.Failed.Add(new RolePermissionResourceResponseStatusDto {
                    PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                    Status = ResponseStatus.NotFound,
                    Field = "RoleId",
                    Message = "Role not found"
                }); continue;
            }

            if (!await _permissionService.ExistById(permissionResourceAndRoleIdsDto.PermissionId)) {
                responsesDto.Failed.Add(new RolePermissionResourceResponseStatusDto {
                    PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                    Status = ResponseStatus.NotFound,
                    Field = "PermissionId",
                    Message = "Permission not found"
                }); continue;
            }

            if (permissionResourceAndRoleIdsDto.ResourceId != null && !await _resourceService.ExistById(permissionResourceAndRoleIdsDto.ResourceId.Value)) {
                responsesDto.Failed.Add(new RolePermissionResourceResponseStatusDto {
                    PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                    Status = ResponseStatus.NotFound,
                    Field = "ResourceId",
                    Message = "Resource not found"
                }); continue;
            }

            RolePermissionResource rolePermissionResource = new RolePermissionResource
            {
                RoleId = permissionResourceAndRoleIdsDto.RoleId,
                PermissionId = permissionResourceAndRoleIdsDto.PermissionId,
                ResourceId = permissionResourceAndRoleIdsDto.ResourceId
            };

            _rolesPermissionsResourcesRepository.Add(rolePermissionResource);
            await _rolesPermissionsResourcesRepository.SaveChangesAsync();
            
            responsesDto.Success.Add(new RolePermissionResourceResponseStatusDto
            {
                PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                Status = ResponseStatus.Success,
                Message = "Success"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignPermissionToRoleAsync and the result was {responsesDto}",
            authenticatedUserId, responsesDto);

        return responsesDto;
    }
}