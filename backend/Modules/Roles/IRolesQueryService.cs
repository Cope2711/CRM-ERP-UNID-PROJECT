using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IRolesQueryService
{
    Task<GetAllResponseDto<Role>> GetAllAsync(GetAllDto getAllDto);
    Task<Role> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Role?> GetById(Guid id);
    Task<Role> GetByNameThrowsNotFoundAsync(string roleName);
    Task<Role?> GetByNameAsync(string roleName);
    Task<bool> ExistRoleNameAsync(string roleName);
    Task<bool> ExistByIdThrowsNotFoundAsync(Guid id);
    Task<bool> ExistById(Guid id);
}