using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersBranchesQueryService
{
    Task<UserBranch?> GetById(Guid id);
    Task<GetAllResponseDto<UserBranch>> GetAll(GetAllDto getAllDto);
    Task<bool> IsUserAssignedToBranch(Guid userId, Guid branchId);
    Task EnsureUserHasAccessToBranch(Guid userId, Guid branchId);
    Task<bool> EnsureUserHasAccessToBranchNotThrows(Guid userId, Guid branchId);
    Task EnsureUserCanModifyUserThrows(Guid authenticatedUserId, Guid affectedUserId);
    Task<bool> EnsureUserCanModifyUserNotThrows(Guid authenticatedUserId, Guid affectedUserId);
}