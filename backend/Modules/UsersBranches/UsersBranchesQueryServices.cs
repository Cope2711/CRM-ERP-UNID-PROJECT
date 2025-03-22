using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class UsersBranchesQueryServices(
    IGenericService<UserBranch> _genericService,
    IUsersBranchesRepository _usersBranchesRepository
    ) : IUsersBranchesQueryService
{
    public async Task<UserBranch?> GetByUserIdAndBranchId(Guid userId, Guid branchId)
    {
        return await _usersBranchesRepository.GetByUserIdAndBranchId(userId, branchId);
    }
    
    public async Task<UserBranch> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id);
    }

    public async Task<GetAllResponseDto<UserBranch>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<bool> ExistByIdThrowsNotFound(Guid id)
    {
        return await _genericService.ExistsAsync(e => e.UserBranchId, id);
    }

    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(e => e.UserBranchId, id);
    }
    
    public async Task<bool> IsUserAssignedToBranch(Guid userId, Guid branchId)
    {
        return await _usersBranchesRepository.IsUserAssignedToBranch(userId, branchId);
    }
}