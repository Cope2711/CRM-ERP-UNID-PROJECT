using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules.Products;

public interface IProductsQueryService
{
    Task<Product> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product> GetByNameThrowsNotFoundAsync(string name);
    Task<Product?> GetByNameAsync(string name);
    Task<GetAllResponseDto<Product>> GetAll(GetAllDto getAllDto);
    Task<bool> ExistByName(string name);
}