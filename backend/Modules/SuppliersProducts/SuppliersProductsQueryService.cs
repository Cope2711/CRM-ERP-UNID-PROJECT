using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class SuppliersProductsQueryService(
    IGenericService<SupplierProduct> _genericService,
    ISuppliersProductsRepository _suppliersProductsRepository
) : ISuppliersProductsQueryService
{
    public async Task<GetAllResponseDto<SupplierProduct>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<SupplierProduct> GetByIdThrowsNotFound(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id,
            query => query
                .Include(sp => sp.Supplier)
                .Include(sp => sp.Product));
    }
    
    public async Task<bool> IsProductAssignedToSupplier(Guid supplierId, Guid productId)
    {
        return await _suppliersProductsRepository.IsProductAssignedToSupplier(supplierId, productId);
    }
    
    public async Task<SupplierProduct?> GetById(Guid id)
    {
        return await _genericService.GetById(id,
            query => query
                .Include(sp => sp.Supplier)
                .Include(sp => sp.Product));
    }
}