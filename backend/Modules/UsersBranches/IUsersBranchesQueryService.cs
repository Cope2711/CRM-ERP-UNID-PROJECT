using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersBranchesQueryService
{
    Task<UserBranch> GetByIdThrowsNotFoundAsync(Guid id);
    Task<GetAllResponseDto<UserBranch>> GetAll(GetAllDto getAllDto);
    Task<bool> ExistByIdThrowsNotFound(Guid id);
    Task<bool> ExistById(Guid id);
    Task<bool> IsUserAssignedToBranch(Guid userId, Guid branchId);
    Task<UserBranch?> GetByUserIdAndBranchId(Guid userId, Guid branchId);
}