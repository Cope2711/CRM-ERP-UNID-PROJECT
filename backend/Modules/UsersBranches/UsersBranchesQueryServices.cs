using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public class UsersBranchesQueryServices(
    IGenericService<UserBranch> _genericService,
    IUsersBranchesRepository _usersBranchesRepository
) : IUsersBranchesQueryService
{
    public async Task<UserBranch?> GetById(Guid id)
    {
        return await _genericService.GetById(id);
    }
    
    public async Task<bool> EnsureUserCanModifyUserNotThrows(Guid authenticatedUserId, Guid affectedUserId)
    {
        return await _usersBranchesRepository.AreUsersInSameBranch(affectedUserId, authenticatedUserId);
    }
    
    public async Task EnsureUserCanModifyUser(Guid authenticatedUserId, Guid affectedUserId)
    {
        if (!await EnsureUserCanModifyUserNotThrows(affectedUserId, authenticatedUserId))
        {
            throw new ForbiddenException("User does not have access to this branch", "NotBranchAccess");
        }
    }
    
    public async Task<bool> EnsureUserHasAccessToBranchNotThrows(Guid userId, Guid branchId)
    {
        return await _usersBranchesRepository.IsUserAssignedToBranch(userId, branchId);
    }
    
    public async Task EnsureUserHasAccessToBranch(Guid userId, Guid branchId)
    {
        if (!await EnsureUserHasAccessToBranchNotThrows(userId, branchId))
        {
            throw new ForbiddenException("User does not have access to this branch", "NotBranchAccess");
        }
    }

    public async Task<UserBranch?> GetByUserIdAndBranchId(Guid userId, Guid branchId)
    {
        return await _usersBranchesRepository.GetByUserIdAndBranchId(userId, branchId);
    }

    public async Task<UserBranch> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFound(id);
    }

    public async Task<GetAllResponseDto<UserBranch>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<bool> IsUserAssignedToBranch(Guid userId, Guid branchId)
    {
        return await _usersBranchesRepository.IsUserAssignedToBranch(userId, branchId);
    }
}