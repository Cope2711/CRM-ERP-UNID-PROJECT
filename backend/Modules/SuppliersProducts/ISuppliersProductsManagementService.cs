using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersProductsManagementService
{
    Task<ResponsesDto<ModelAndAssignResponseStatusDto>> AssignProductsToSuppliers(ModelsAndAssignsDtos modelsAndAssignsDtos);
    Task<ResponsesDto<IdResponseStatusDto>> RevokeProductsFromSuppliers(IdsDto idsDto);
    Task<SupplierProduct> Update(UpdateSupplierProductDto updateSupplierProductDto);
}