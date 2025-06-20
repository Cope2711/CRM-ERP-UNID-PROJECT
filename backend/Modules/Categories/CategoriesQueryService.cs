using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class CategoriesQueryService(
    IGenericService<Category> _genericService
) : ICategoriesQueryService
{
    public async Task<bool> ExistsById(Guid categoryId)
    {
        return await _genericService.ExistsAsync(query => query.id == categoryId);
    }

    public async Task<bool> ExistsByName(string categoryName)
    {
        return await _genericService.ExistsAsync(query => query.name == categoryName);
    }

    public async Task<Category> GetByIdThrowsNotFound(Guid categoryId)
    {
        return await _genericService.GetByIdThrowsNotFound(categoryId);
    }

    public async Task<GetAllResponseDto<Category>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }
}