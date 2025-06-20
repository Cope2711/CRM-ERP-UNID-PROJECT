using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class PriorityValidationService(
    IHttpContextAccessor _httpContextAccessor,
    IUsersRolesQueryService _usersRolesQueryService,
    IRolesQueryService _rolesQueryService
) : IPriorityValidationService
{
    public bool ValidateUserPriority(User user) =>
        IsPriorityValid(user.ToUserRolesPriority());

    public async Task<bool> ValidateUserPriorityById(Guid userId)
    {
        var authMaxRolePriority = GetAuthenticatedUserMaxRolePriority();

        double? targetUserPriority = await _usersRolesQueryService.GetMaxRolePriorityByUserId(userId);

        return IsPriorityGreater(authMaxRolePriority, targetUserPriority);
    }

    public async Task<bool> ValidateRolePriorityById(Guid roleId)
    {
        var authMaxRolePriority = GetAuthenticatedUserMaxRolePriority();

        double rolePriority = await _rolesQueryService.GetRolePriorityById(roleId);
        return IsPriorityGreater(authMaxRolePriority, rolePriority);
    }

    public void ValidateRolePriorityThrowsForbiddenException(Role role) =>
        ValidatePriorityOrThrow(role.priority);

    public void ValidateUserPriorityThrowsForbiddenException(User user) =>
        ValidatePriorityOrThrow(user.ToUserRolesPriority());

    public void ValidatePriorityThrowsForbiddenException(params double[] targetPriorities)
    {
        var authMaxRolePriority = GetAuthenticatedUserMaxRolePriority();

        if (targetPriorities.Any() && !IsPriorityGreater(authMaxRolePriority, targetPriorities.Max()))
            throw new ForbiddenException("Not enough permission to make the action", field: Fields.Roles.priority);
    }

    private bool IsPriorityValid(params double[] targetPriorities)
    {
        try
        {
            ValidatePriorityOrThrow(targetPriorities);
            return true;
        }
        catch (ForbiddenException)
        {
            return false;
        }
    }

    private void ValidatePriorityOrThrow(params double[] targetPriorities) =>
        ValidatePriorityThrowsForbiddenException(targetPriorities);

    private double GetAuthenticatedUserMaxRolePriority() =>
        HttpContextHelper.GetAuthenticatedUserMaxRolePriority(_httpContextAccessor);

    private bool IsPriorityGreater(double authPriority, double? targetPriority)
    {
        if (targetPriority == null)
        {
            return true;
        }
        
        return authPriority > targetPriority;
    }
}
