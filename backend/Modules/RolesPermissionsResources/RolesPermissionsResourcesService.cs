using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class RolesPermissionsResourcesService(
    IRolesPermissionsResourcesRepository _rolesPermissionsResourcesRepository,
    IPermissionService _permissionService,
    IGenericService<RolePermissionResource> _genericService,
    IResourceService _resourceService,
    ILogger<RolesPermissionsResourcesService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IPriorityValidationService _priorityValidationService,
    IRolesQueryService _rolesQueryService
) : IRolesPermissionsResourcesService
{
    public async Task<ResponsesDto<IdResponseStatusDto>> RevokePermissionsToRolesAsync(
        IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokePermissionToRoleAsync with the object {permissionsResourcesAndRolesIdsDto}",
            authenticatedUserId, idsDto);

        foreach (Guid rolePermissionResourceId in idsDto.Ids)
        {
            RolePermissionResource? rolePermissionResource =
                await GetById(rolePermissionResourceId);

            if (rolePermissionResource == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, rolePermissionResourceId, ResponseStatus.NotFound,
                    Fields.Permissions.id, "Permission not found"); continue;
            }
            
            if (!await _priorityValidationService.ValidateRolePriorityById(rolePermissionResource.roleId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, rolePermissionResourceId, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.id, "Not have enough priority to modify that role"); continue;
            }

            // Remove from database
            _rolesPermissionsResourcesRepository.Remove(rolePermissionResource);
            await _rolesPermissionsResourcesRepository.SaveChangesAsync();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = rolePermissionResourceId,
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
            if (await ArePermissionIdResourceIdAssignedToRoleIdAsync(permissionResourceAndRoleIdsDto.roleId,
                    permissionResourceAndRoleIdsDto.permissionId, permissionResourceAndRoleIdsDto.resourceId))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.AlreadyProcessed,
                    Fields.Permissions.id, "Already assigned"); continue;
            }
            
            if (!await _rolesQueryService.ExistById(permissionResourceAndRoleIdsDto.roleId))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotFound,
                    Fields.Roles.id, "Role not found"); continue;
            }

            if (!await _permissionService.ExistById(permissionResourceAndRoleIdsDto.permissionId))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotFound,
                    Fields.Permissions.id, "Permission not found"); continue;
            }

            if (permissionResourceAndRoleIdsDto.resourceId != null &&
                !await _resourceService.ExistById(permissionResourceAndRoleIdsDto.resourceId.Value))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotFound,
                    Fields.Resources.id, "Resource not found"); continue;
            }

            if (!await _priorityValidationService.ValidateRolePriorityById(permissionResourceAndRoleIdsDto.roleId))
            {
                AddFailedResponseDto(responseDto, permissionResourceAndRoleIdsDto, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.id, "Not have enough priority to modify that role"); continue;
            }

            RolePermissionResource rolePermissionResource = new RolePermissionResource
            {
                roleId = permissionResourceAndRoleIdsDto.roleId,
                permissionId = permissionResourceAndRoleIdsDto.permissionId,
                resourceId = permissionResourceAndRoleIdsDto.resourceId
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
            throw new NotFoundException("They are not assigned to role.", field: Fields.Permissions.id);
        }

        return rolePermissionResource;
    }
    
    public async Task<RolePermissionResource?> GetById(Guid id)
    {
        return await _genericService.GetById(id);
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