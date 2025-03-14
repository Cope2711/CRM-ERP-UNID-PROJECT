using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IPriorityValidationService
{
    bool ValidateRolePriority(Role role);
    bool ValidateUserPriority(User user);
    void ValidateRolePriorityThrowsForbiddenException(Role role);
    void ValidateUserPriorityThrowsForbiddenException(User user);
    void ValidatePriorityThrowsForbiddenException(params double[] rolePriorities);
}