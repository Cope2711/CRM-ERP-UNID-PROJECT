using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class SuppliersQueryServices(
    IGenericService<Supplier> genericService
) : ISuppliersQueryService
{
    public async Task<Supplier?> GetById(Guid id)
    {
        return await genericService.GetById(id);
    }
    
    public async Task<Supplier> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await genericService.GetByIdThrowsNotFound(id);
    }

    public async Task<GetAllResponseDto<Supplier>> GetAll(GetAllDto getAllDto)
    {
        return await genericService.GetAllAsync(getAllDto);
    }

    public async Task<bool> ExistByIdThrowsNotFound(Guid id)
    {
        return await genericService.ExistsAsync(s => s.id == id);
    }

    public async Task<bool> ExistById(Guid id)
    {
        return await genericService.ExistsAsync(s => s.id == id);
    }

    public async Task<bool> ExistByName(string supplierName)
    {
        return await genericService.ExistsAsync(s => s.name == supplierName);
    }
    
    public async Task<bool> ExistByEmail(string supplierEmail)
    {
        return await genericService.ExistsAsync(s => s.email == supplierEmail);
    }
}