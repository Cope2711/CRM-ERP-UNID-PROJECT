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
    IRolesRepository rolesRepository
) : IRolesManagementService
{
    public async Task<Role> DeleteById(Guid id)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with Id {authenticatedUserId} requested DeleteById for RoleId {TargetRoleId}",
            authenticatedUserId, id);

        Role role = await _rolesQueryService.GetByIdThrowsNotFound(id);
        _priorityValidationService.ValidateRolePriorityThrowsForbiddenException(role);
        rolesRepository.Remove(role);
        await rolesRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested DeleteById for RoleId {TargetRoleId} and the role was deleted",
            authenticatedUserId, id);

        return role;
    }

    public async Task<Role> CreateRole(CreateRoleDto createRoleDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateRoleAsync for RoleName {TargetRoleName}",
            authenticatedUserId, createRoleDto.RoleName);

        _priorityValidationService.ValidatePriorityThrowsForbiddenException(createRoleDto.RolePriority);

        // Exist roleName?
        if (await _rolesQueryService.ExistRoleName(createRoleDto.RoleName))
        {
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateRoleAsync for RoleName {TargetRoleName} but the rolename already exists",
                authenticatedUserId, createRoleDto.RoleName);

            throw new UniqueConstraintViolationException(
                $"A role with the name '{createRoleDto.RoleName}' already exists.",
                field: "RoleName");
        }

        Role newRole = new Role
        {
            RoleName = createRoleDto.RoleName,
            RolePriority = createRoleDto.RolePriority,
            RoleDescription = createRoleDto.RoleDescription,
        };

        rolesRepository.Add(newRole);
        await rolesRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateRoleAsync for RoleName {TargetRoleName} and the role was created",
            authenticatedUserId, createRoleDto.RoleName);

        return newRole;
    }

    public async Task<Role> Update(UpdateRoleDto updateRoleDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Role role = await _rolesQueryService.GetByIdThrowsNotFound(updateRoleDto.RoleId);

        if (authenticatedUserId != updateRoleDto.RoleId)
            _priorityValidationService.ValidateRolePriorityThrowsForbiddenException(role);

        bool hasChanges = ModelsHelper.UpdateModel(role, updateRoleDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateRoleDto.RoleName):
                    return await _rolesQueryService.ExistRoleName((string)value);

                case nameof(updateRoleDto.RolePriority):
                    _priorityValidationService.ValidatePriorityThrowsForbiddenException((double)value);
                    return true;

                default:
                    return false;
            }
        });

        if (hasChanges)
        {
            await rolesRepository.SaveChangesAsync();
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for RoleId {TargetRoleId} and the role was updated",
                authenticatedUserId, updateRoleDto.RoleId);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for RoleId {TargetRoleId} and the role was not updated",
                authenticatedUserId, updateRoleDto.RoleId);
        }

        return role;
    }
}