using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersBranchesManagementService
{
    Task<ResponsesDto<ModelAndAssignResponseStatusDto>> AssignBranchToUser(ModelsAndAssignsDtos modelsAndAssignsDtos);
    Task<ResponsesDto<IdResponseStatusDto>> RevokeBranchToUser(IdsDto idsDto);
}