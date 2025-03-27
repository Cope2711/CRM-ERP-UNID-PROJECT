using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersProductsQueryService
{
    Task<GetAllResponseDto<SupplierProduct>> GetAll(GetAllDto getAllDto);
    Task<SupplierProduct> GetByIdThrowsNotFound(Guid id);
    Task<SupplierProduct?> GetById(Guid id);
    Task<bool> IsProductAssignedToSupplier(Guid supplierId, Guid productId);
}