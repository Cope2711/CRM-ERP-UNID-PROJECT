using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IProductsQueryService
{
    Task<Product> GetByIdThrowsNotFound(Guid id);
    Task<Product?> GetById(Guid id);
    Task<Product> GetByNameThrowsNotFound(string name);
    Task<Product> GetByBarcodeThrowsNotFound(string barcode);
    Task<Product?> GetByName(string name);
    Task<GetAllResponseDto<Product>> GetAll(GetAllDto getAllDto);
    Task<bool> ExistByName(string name);
    Task<bool> ExistById(Guid id);
    Task<bool> ExistByIdThrowsNotFound(Guid id);
    Task<bool> ExistByBarcode(string barcode);
}