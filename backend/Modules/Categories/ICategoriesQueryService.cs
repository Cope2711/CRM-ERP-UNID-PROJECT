using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ICategoriesQueryService
{
    Task<Category> GetByIdThrowsNotFound(Guid categoryId);
    Task<GetAllResponseDto<Category>> GetAll(GetAllDto getAllDto);
    Task<bool> ExistsByName(string categoryName);
    Task<bool> ExistsById(Guid categoryId);
}