using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public class RolesQueryService(
    IGenericService<Role> _genericService,
    IRolesRepository _rolesRepository
) : IRolesQueryService
{
    public async Task<double> GetRolePriorityById(Guid roleId)
    {
        double? rolePriority = await _rolesRepository.GetRolePriorityById(roleId);
        if (rolePriority == null)
        {
            throw new NotFoundException("Role not exist", field: "RoleId");
        }
        return rolePriority.Value;
    }
    
    public async Task<bool> ExistRoleName(string roleName)
    {
        return await _genericService.ExistsAsync(r => r.name == roleName);
    }

    public async Task<GetAllResponseDto<Role>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<Role> GetByIdThrowsNotFound(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFound(id);
    }

    public async Task<Role?> GetById(Guid id)
    {
        return await _genericService.GetById(id);
    }

    public async Task<Role> GetByNameThrowsNotFound(string roleName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(r => r.name, roleName);
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _genericService.GetFirstAsync(r => r.name, roleName);
    }

    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(r => r.id == id);
    }
}