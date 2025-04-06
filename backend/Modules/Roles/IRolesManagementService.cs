using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IRolesManagementService
{
    Task<Role> CreateRole(CreateRoleDto createRoleDto);
    Task<Role> Update(UpdateRoleDto updateRoleDto);
    Task<Role> DeleteById(Guid id);
}