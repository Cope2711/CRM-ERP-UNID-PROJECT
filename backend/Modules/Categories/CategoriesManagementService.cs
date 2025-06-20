using System.Text.Json;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class CategoriesManagementService(
    ICategoriesQueryService _categoriesQueryService,
    ICategoriesRepository _categoriesRepository,
    ILogger<CategoriesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IGenericService<Category> _genericService
    ) : ICategoriesManagementService
{
    public async Task<Category> Create(Category data)
    {
        return await _genericService.Create(data);
    }
    
    public async Task<Category> Update(Guid id, JsonElement data)
    {
        Category category = await _categoriesQueryService.GetByIdThrowsNotFound(id);
        
        await _genericService.Update(category, data);
        
        return category;
    }
    
    public async Task<Category> Delete(Guid categoryId)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Category category = await _categoriesQueryService.GetByIdThrowsNotFound(categoryId);
        _logger.LogInformation("User with Id {authenticatedUserId} requested Delete for CategoryId {TargetCategoryId}", authenticatedUserId, categoryId);

        _categoriesRepository.Remove(category);
        await _categoriesRepository.SaveChanges();
        
        _logger.LogInformation("User with Id {authenticatedUserId} requested Delete for CategoryId {TargetCategoryId} and the category was deleted", authenticatedUserId, categoryId);
        return category;
    }
}