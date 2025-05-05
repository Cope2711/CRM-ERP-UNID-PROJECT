using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
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
    public async Task<Category> Create(CreateCategoryDto createCategoryDto)
    {
        return await _genericService.Create(createCategoryDto.ToModel());
    }
    
    public async Task<Category> Update(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Category category = await _categoriesQueryService.GetByIdThrowsNotFound(id);
        _logger.LogInformation("User with Id {authenticatedUserId} requested Update for CategoryId {TargetCategoryId}", authenticatedUserId, id);
        
        bool hasChanges = ModelsHelper.UpdateModel(category, updateCategoryDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateCategoryDto.CategoryName):
                    return await _categoriesQueryService.ExistsByName((string)value);
                
                default:
                    return false;
            }
        });
        
        if (hasChanges)
        {
            await _categoriesRepository.SaveChanges();
            _logger.LogInformation("User with Id {authenticatedUserId} requested Update for CategoryId {TargetCategoryId} and the category was updated", authenticatedUserId, id);
        }
        else
        {
            _logger.LogInformation("User with Id {authenticatedUserId} requested Update for CategoryId {TargetCategoryId} and the category was not updated", authenticatedUserId, id);
        }
        
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