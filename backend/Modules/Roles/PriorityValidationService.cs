using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public interface IPriorityValidationService
{
    bool ValidateRolePriority(Role role);
    bool ValidateUserPriority(User user);
    void ValidateRolePriorityThrowsForbiddenException(Role role);
    void ValidateUserPriorityThrowsForbiddenException(User user);
    void ValidatePriorityThrowsForbiddenException(params double[] rolePriorities);
}

public class PriorityValidationService(
    IHttpContextAccessor _httpContextAccessor
) : IPriorityValidationService
{
    public bool ValidateRolePriority(Role role) 
        => ValidatePriorityWithoutException(role.RolePriority);

    public bool ValidateUserPriority(User user) 
        => ValidatePriorityWithoutException(Mapper.UserToUserRolesPriority(user));

    public void ValidateRolePriorityThrowsForbiddenException(Role role)
    {
        ValidatePriorityThrowsForbiddenException(role.RolePriority);
    }
    
    public void ValidateUserPriorityThrowsForbiddenException(User user)
    {
        ValidatePriorityThrowsForbiddenException(Mapper.UserToUserRolesPriority(user));
    }
    
    public void ValidatePriorityThrowsForbiddenException(params double[] rolePriorities)
    {
        double[] authenticatedUserRolePriorities = HttpContextHelper.GetAuthenticatedUserRolePriorities(_httpContextAccessor);
        
        if (!authenticatedUserRolePriorities.Any())
            throw new ForbiddenException("Authenticated user has no roles assigned");

        if (!rolePriorities.Any())
            return;

        bool hasPriority = authenticatedUserRolePriorities.Any(authPriority =>
            rolePriorities.Any(rolePriority => authPriority > rolePriority));

        if (!hasPriority)
            throw new ForbiddenException("Not enough permission to make the action");
    }

    private bool ValidatePriorityWithoutException(params double[] rolePriorities)
    {
        try
        {
            ValidatePriorityThrowsForbiddenException(rolePriorities);
            return true;
        }
        catch (ForbiddenException)
        {
            return false;
        }
    }
}
