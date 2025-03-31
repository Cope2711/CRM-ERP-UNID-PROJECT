using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersBranchesManagementService
{
    Task<ResponsesDto<UserBranchResponseStatusDto>> AssignBranchToUserAsync(UsersAndBranchesDtos usersAndBranchesDtos);
    Task<ResponsesDto<IdResponseStatusDto>> RevokeBranchToUserAsync(IdsDto idsDto);
}