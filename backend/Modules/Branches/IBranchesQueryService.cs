using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IBranchesQueryService
{
    Task<Branch> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Branch> GetByNameThrowsNotFoundAsync(string name);
    Task<GetAllResponseDto<Branch>> GetAll(GetAllDto getAllDto);
    Task<bool> ExistByName(string name);
    Task<bool> ExistsByIdThrowsNotFound(Guid id);
}