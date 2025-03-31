using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IPriorityValidationService
{
    bool ValidateUserPriority(User user);
    Task<bool> ValidateUserPriorityById(Guid userId);
    Task<bool> ValidateRolePriorityById(Guid roleId);
    void ValidateRolePriorityThrowsForbiddenException(Role role);
    void ValidateUserPriorityThrowsForbiddenException(User user);
    void ValidatePriorityThrowsForbiddenException(params double[] rolePriorities);
}