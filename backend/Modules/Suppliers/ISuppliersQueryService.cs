using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersQueryService
{
    Task<Supplier> GetByIdThrowsNotFoundAsync(Guid id);
    Task<GetAllResponseDto<Supplier>> GetAll(GetAllDto getAllDto);
    Task<bool> ExistByIdThrowsNotFound(Guid id);
    Task<bool> ExistById(Guid id);
    Task<bool> ExistByName(string exampleName);
    Task<bool> ExistByEmail(string supplierEmail);
    Task<Supplier?> GetById(Guid id);
}