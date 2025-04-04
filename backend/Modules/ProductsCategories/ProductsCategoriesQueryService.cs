using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class ProductsCategoriesQueryService(
    IGenericService<ProductCategory> _genericService,
    IProductsCategoriesRepository _productsCategoriesRepository
) : IProductsCategoriesQueryService
{
    public Task<ProductCategory?> GetById(Guid productCategoryId)
    {
        return _genericService.GetById(productCategoryId);
    }

    public async Task<bool> ExistsByProductCategoryIds(Guid productId, Guid categoryId)
    {
        return await _productsCategoriesRepository.ExistsByProductCategoryIds(productId, categoryId);
    }
    
    public async Task<GetAllResponseDto<ProductCategory>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }
}