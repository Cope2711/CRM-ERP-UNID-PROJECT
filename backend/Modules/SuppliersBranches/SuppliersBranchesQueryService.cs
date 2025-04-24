using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class SuppliersBranchesQueryService(
    IGenericService<SupplierBranch> _genericService,
    ISuppliersBranchesRepository _suppliersBranchesRepository
) : ISuppliersBranchesQueryService
{
    public async Task<GetAllResponseDto<SupplierBranch>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<SupplierBranch> GetByIdThrowsNotFound(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFound(id, 
            query => query.Include(sb => sb.Supplier).Include(sb => sb.Branch));
    }

    public async Task<SupplierBranch?> GetById(Guid id)
    {
        return await _genericService.GetById(id);
    }
    
    public async Task<bool> IsSupplierAssignedToBranch(Guid supplierId, Guid branchId)
    {
        return await _suppliersBranchesRepository.IsSupplierAssignedToBranch(supplierId, branchId);
    }
}