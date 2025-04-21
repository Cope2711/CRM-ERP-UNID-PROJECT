using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IProductsCategoriesManagementService
{
    Task<ResponsesDto<ModelAndAssignResponseStatusDto>> Assign(ModelsAndAssignsDtos modelsAndAssignsDtos);
    Task<ResponsesDto<IdResponseStatusDto>> Revoke(IdsDto idsDto);
}