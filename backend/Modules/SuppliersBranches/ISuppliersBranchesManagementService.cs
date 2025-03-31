using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersBranchesManagementService
{
    Task<ResponsesDto<SuppliersBranchResponseStatusDto>> AssignBranchesToSuppliers(SuppliersAndBranchesDto suppliersAndBranchesDto);
    Task<ResponsesDto<IdResponseStatusDto>> RevokeBranchesFromSuppliers(IdsDto idsDto);
    Task<SupplierBranch> Update(UpdateSupplierBranchDto updateSupplierBranchDto);
}