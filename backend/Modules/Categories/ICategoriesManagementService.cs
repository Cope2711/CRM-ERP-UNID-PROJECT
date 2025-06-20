using System.Text.Json;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface ICategoriesManagementService
{
    Task<Category> Create(Category data);
    Task<Category> Update(Guid id, JsonElement data);
    Task<Category> Delete(Guid categoryId);
}