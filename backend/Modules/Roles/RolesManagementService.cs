using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class RolesManagementService(
    ILogger<RolesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IRolesQueryService _rolesQueryService,
    IPriorityValidationService _priorityValidationService,
    IRolesRepository _rolesRepository,
    IGenericService<Role> _genericService
    ) : IRolesManagementService
{
    public async Task<Role> DeleteById(Guid id)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with Id {authenticatedUserId} requested DeleteById for RoleId {TargetRoleId}",
            authenticatedUserId, id);

        Role role = await _rolesQueryService.GetByIdThrowsNotFound(id);
        _priorityValidationService.ValidateRolePriorityThrowsForbiddenException(role);
        _rolesRepository.Remove(role);
        await _rolesRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested DeleteById for RoleId {TargetRoleId} and the role was deleted",
            authenticatedUserId, id);

        return role;
    }

    public async Task<Role> Create(CreateRoleDto createRoleDto)
    {
        _priorityValidationService.ValidatePriorityThrowsForbiddenException(createRoleDto.RolePriority);

        return await _genericService.Create(createRoleDto.ToModel());
    }

    public async Task<Role> Update(Guid id, UpdateRoleDto updateRoleDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Role role = await _rolesQueryService.GetByIdThrowsNotFound(id);

        if (authenticatedUserId != id)
            _priorityValidationService.ValidateRolePriorityThrowsForbiddenException(role);

        if (updateRoleDto.RolePriority != null &&
            updateRoleDto.RolePriority != role.RolePriority)
        {
            _priorityValidationService.ValidatePriorityThrowsForbiddenException(updateRoleDto.RolePriority.Value);
        }

        await _genericService.Update(role, updateRoleDto);

        return role;
    }
}