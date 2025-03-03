using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsResourcesService
{
    Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName,
        string? resourceName = null);

    Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);

    Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> AssignPermissionsToRolesAsync(
        PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto);

    Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> RevokePermissionsToRolesAsync(
        PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto);

    Task<GetAllResponseDto<RolePermissionResource>> GetAllAsync(GetAllDto getAllDto);
}

public class RolesPermissionsResourcesService(
    IRolesPermissionsResourcesRepository _rolesPermissionsResourcesRepository,
    IRoleService _roleService,
    IPermissionService _permissionService,
    IGenericServie<RolePermissionResource> _genericService,
    IResourceService _resourceService,
    ILogger<RolesPermissionsResourcesService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IPriorityValidationService _priorityValidationService
) : IRolesPermissionsResourcesService
{
    public async Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> RevokePermissionsToRolesAsync(
        PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<RolePermissionResourceResponseStatusDto> responseDto = new();

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
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotFound,
                    "PermissionId", "Permission not found"); continue;
            }

            Role role = await _roleService.GetByIdThrowsNotFoundAsync(permissionResourceAndRoleIdsDto.RoleId);

            if (!_priorityValidationService.ValidateRolePriority(role))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotEnoughPriority,
                    "RoleId", "Not have enough priority to modify that role"); continue;
            }

            // Remove from database
            _rolesPermissionsResourcesRepository.Remove(rolePermissionResource);
            await _rolesPermissionsResourcesRepository.SaveChangesAsync();

            responseDto.Success.Add(new RolePermissionResourceResponseStatusDto
            {
                PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                Status = ResponseStatus.Success,
                Message = "Success"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignPermissionToRoleAsync and the result was {responsesDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> AssignPermissionsToRolesAsync(
        PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<RolePermissionResourceResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignPermissionToRoleAsync with the object {permissionsResourcesAndRolesIdsDto}",
            authenticatedUserId, permissionsResourcesAndRolesIdsDto);

        foreach (PermissionResourceAndRoleIdsDto permissionResourceAndRoleIdsDto in permissionsResourcesAndRolesIdsDto
                     .PermissionResourceAndRoleIds)
        {
            if (await ArePermissionIdResourceIdAssignedToRoleIdAsync(permissionResourceAndRoleIdsDto.RoleId,
                    permissionResourceAndRoleIdsDto.PermissionId, permissionResourceAndRoleIdsDto.ResourceId))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.AlreadyProcessed,
                    "PermissionId", "Already assigned"); continue;
            }

            Role? role = await _roleService.GetById(permissionResourceAndRoleIdsDto.RoleId);

            if (role == null)
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotFound,
                    "RoleId", "Role not found"); continue;
            }

            if (!await _permissionService.ExistById(permissionResourceAndRoleIdsDto.PermissionId))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotFound,
                    "PermissionId", "Permission not found"); continue;
            }

            if (permissionResourceAndRoleIdsDto.ResourceId != null &&
                !await _resourceService.ExistById(permissionResourceAndRoleIdsDto.ResourceId.Value))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotFound,
                    "ResourceId", "Resource not found"); continue;
            }

            if (!_priorityValidationService.ValidateRolePriority(role))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotEnoughPriority,
                    "RoleId", "Not have enough priority to modify that role"); continue;
            }

            RolePermissionResource rolePermissionResource = new RolePermissionResource
            {
                RoleId = permissionResourceAndRoleIdsDto.RoleId,
                PermissionId = permissionResourceAndRoleIdsDto.PermissionId,
                ResourceId = permissionResourceAndRoleIdsDto.ResourceId
            };

            _rolesPermissionsResourcesRepository.Add(rolePermissionResource);
            await _rolesPermissionsResourcesRepository.SaveChangesAsync();

            responseDto.Success.Add(new RolePermissionResourceResponseStatusDto
            {
                PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
                Status = ResponseStatus.Success,
                Message = "Success"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignPermissionToRoleAsync and the result was {responsesDto}",
            authenticatedUserId, responseDto);

        return responseDto;
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

    private void AddFailedResponseDto(ResponsesDto<RolePermissionResourceResponseStatusDto> responseDto,
        PermissionResourceAndRoleIdsDto permissionResourceAndRoleIdsDto, string status, string field, string message)
    {
        responseDto.Failed.Add(new RolePermissionResourceResponseStatusDto
        {
            PermissionResourceAndRoleIds = permissionResourceAndRoleIdsDto,
            Status = status,
            Field = field,
            Message = message
        });
    }
}