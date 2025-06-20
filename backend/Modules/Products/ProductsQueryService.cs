using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class ProductsQueryService(
    IGenericService<Product> _genericService
) : IProductsQueryService
{
    public Task<Product> GetByIdThrowsNotFound(Guid id)
    {
        return _genericService.GetByIdThrowsNotFound(id, 
            query => query.Include(p => p.ProductCategories).ThenInclude(pc => pc.Category));
    }

    public Task<Product?> GetById(Guid id)
    {
        return _genericService.GetById(id);
    }

    public Task<Product> GetByNameThrowsNotFound(string name)
    {
        return _genericService.GetFirstThrowsNotFoundAsync(p => p.name, name);
    }

    public Task<Product> GetByBarcodeThrowsNotFound(string barcode)
    {
        return _genericService.GetFirstThrowsNotFoundAsync(p => p.barcode, barcode);
    }

    public Task<Product?> GetByName(string name)
    {
        return _genericService.GetFirstAsync(p => p.name, name);
    }

    public Task<GetAllResponseDto<Product>> GetAll(GetAllDto getAllDto)
    {
        return _genericService.GetAllAsync(getAllDto);
    }
    
    public Task<bool> ExistByName(string name)
    {
        return _genericService.ExistsAsync(p => p.name == name);
    }
    
    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(p => p.id == id);
    }

    public async Task<bool> ExistByIdThrowsNotFound(Guid id)
    {
        if (!await ExistById(id))
        {
            throw new NotFoundException(message: "Product not found", field: Fields.Sales.id);
        }
        return true;
    }

    public async Task<bool> ExistByBarcode(string barcode)
    {
        return await _genericService.ExistsAsync(p => p.barcode == barcode);
    }
}