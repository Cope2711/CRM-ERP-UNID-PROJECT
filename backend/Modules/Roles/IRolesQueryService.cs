using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IRolesQueryService
{
    Task<GetAllResponseDto<Role>> GetAll(GetAllDto getAllDto);
    Task<Role> GetByIdThrowsNotFound(Guid id);
    Task<Role?> GetById(Guid id);
    Task<Role> GetByNameThrowsNotFound(string roleName);
    Task<bool> ExistRoleName(string roleName);
    Task<bool> ExistById(Guid id);
    Task<double> GetRolePriorityById(Guid roleId);
}