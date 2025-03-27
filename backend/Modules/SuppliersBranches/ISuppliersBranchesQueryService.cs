using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersBranchesQueryService
{
    Task<GetAllResponseDto<SupplierBranch>> GetAll(GetAllDto getAllDto);
    Task<SupplierBranch> GetByIdThrowsNotFound(Guid id);
    Task<SupplierBranch?> GetById(Guid id);
    Task<bool> IsSupplierAssignedToBranch(Guid supplierId, Guid branchId);
}