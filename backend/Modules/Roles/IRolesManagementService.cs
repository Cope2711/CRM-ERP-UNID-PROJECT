using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IRolesManagementService
{
    Task<Role> Create(CreateRoleDto createRoleDto);
    Task<Role> Update(Guid id, UpdateRoleDto updateRoleDto);
    Task<Role> DeleteById(Guid id);
}