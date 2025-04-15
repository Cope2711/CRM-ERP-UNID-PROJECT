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
    IHttpContextAccessor _httpContextAccessor
) : ICategoriesManagementService
{
    public async Task<Category> Create(CreateCategoryDto createCategoryDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        _logger.LogInformation("User with Id {authenticatedUserId} requested Create for CategoryName {TargetCategoryName}", authenticatedUserId, createCategoryDto.CategoryName);

        if (await _categoriesQueryService.ExistsByName(createCategoryDto.CategoryName))
        {
            _logger.LogError("User with Id {authenticatedUserId} requested Create for CategoryName {TargetCategoryName} but the category already exists", authenticatedUserId, createCategoryDto.CategoryName);
            throw new UniqueConstraintViolationException("Category with this name already exists", Fields.Categories.CategoryName);
        }
        
        Category category = new()
        {
            CategoryName = createCategoryDto.CategoryName,
            CategoryDescription = createCategoryDto.CategoryDescription
        };

        _categoriesRepository.Add(category);
        await _categoriesRepository.SaveChanges();
        
        _logger.LogInformation("User with Id {authenticatedUserId} requested Create for CategoryName {TargetCategoryName} and the category was created", authenticatedUserId, createCategoryDto.CategoryName);
        return category;
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