using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IRolesManagementService
{
    Task<Role> CreateRoleAsync(CreateRoleDto createRoleDto);
    Task<Role> UpdateAsync(UpdateRoleDto updateRoleDto);
    Task<Role> DeleteById(Guid id);
}