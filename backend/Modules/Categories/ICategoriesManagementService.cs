using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ICategoriesManagementService
{
    Task<Category> Create(CreateCategoryDto createCategoryDto);
    Task<Category> Update(Guid id, UpdateCategoryDto updateCategoryDto);
    Task<Category> Delete(Guid categoryId);
}