using System.Text.Json;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
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

    public async Task<Role> Create(Role data)
    {
        _priorityValidationService.ValidatePriorityThrowsForbiddenException(data.priority);

        return await _genericService.Create(data);
    }

    public async Task<Role> Update(Guid id, JsonElement data)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        Role role = await _rolesQueryService.GetByIdThrowsNotFound(id);

        if (authenticatedUserId != id)
            _priorityValidationService.ValidateRolePriorityThrowsForbiddenException(role);

        string rolePriorityKey = Fields.Roles.priority; 

        if (data.TryGetProperty(rolePriorityKey, out var rolePriorityElement) &&
            rolePriorityElement.ValueKind != JsonValueKind.Null)
        {
            double newPriority = rolePriorityElement.GetDouble();
            _priorityValidationService.ValidatePriorityThrowsForbiddenException(newPriority);
        }

        await _genericService.Update(role, data);

        return role;
    }
}