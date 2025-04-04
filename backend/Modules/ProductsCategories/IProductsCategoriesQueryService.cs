using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IProductsCategoriesQueryService
{
    Task<ProductCategory?> GetById(Guid productCategoryId);
    Task<bool> ExistsByProductCategoryIds(Guid productId, Guid categoryId);
    Task<GetAllResponseDto<ProductCategory>> GetAll(GetAllDto getAllDto);
}