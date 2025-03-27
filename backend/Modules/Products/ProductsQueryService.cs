using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class ProductsQueryService(
    IGenericService<Product> _genericService
) : IProductsQueryService
{
    public Task<Product> GetByIdThrowsNotFound(Guid id)
    {
        return _genericService.GetByIdThrowsNotFoundAsync(id);
    }

    public Task<Product?> GetById(Guid id)
    {
        return _genericService.GetById(id);
    }

    public Task<Product> GetByNameThrowsNotFound(string name)
    {
        return _genericService.GetFirstThrowsNotFoundAsync(p => p.ProductName, name);
    }

    public Task<Product?> GetByName(string name)
    {
        return _genericService.GetFirstAsync(p => p.ProductName, name);
    }

    public Task<GetAllResponseDto<Product>> GetAll(GetAllDto getAllDto)
    {
        return _genericService.GetAllAsync(getAllDto);
    }
    
    public Task<bool> ExistByName(string name)
    {
        return _genericService.ExistsAsync(p => p.ProductName, name);
    }
    
    public Task<bool> ExistById(Guid id)
    {
        return _genericService.ExistsAsync(p => p.ProductId, id);
    }
}