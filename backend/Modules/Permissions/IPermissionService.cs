using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IPermissionService
{
    Task<GetAllResponseDto<Permission>> GetAllAsync(GetAllDto getAllDto);
    Task<Permission> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Permission?> GetByNameAsync(string permissionName);
    Task<bool> ExistById(Guid id);
}