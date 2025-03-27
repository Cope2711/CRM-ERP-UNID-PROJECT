using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersBranchesManagementService
{
    Task<ResponsesDto<SuppliersBranchResponseStatusDto>> AssignBranchesToSuppliers(SuppliersAndBranchesDto suppliersAndBranchesDto);
    Task<ResponsesDto<SuppliersBranchesRevokedResponseStatusDto>> RevokeBranchesFromSuppliers(SuppliersBranchesIdsDto suppliersBranchesIdsDto);
    Task<SupplierBranch> Update(UpdateSupplierBranchDto updateSupplierBranchDto);
}