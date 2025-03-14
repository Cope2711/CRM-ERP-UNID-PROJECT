using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public class RolesQueryService(
    IGenericService<Role> _genericService
) : IRolesQueryService
{
    public async Task<bool> ExistRoleNameAsync(string roleName)
    {
        return await _genericService.ExistsAsync(r => r.RoleName, roleName);
    }

    public async Task<GetAllResponseDto<Role>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<Role> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id);
    }

    public async Task<Role?> GetById(Guid id)
    {
        return await _genericService.GetById(id);
    }

    public async Task<Role> GetByNameThrowsNotFoundAsync(string roleName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(r => r.RoleName, roleName);
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _genericService.GetFirstAsync(r => r.RoleName, roleName);
    }

    public async Task<bool> ExistByIdThrowsNotFoundAsync(Guid id)
    {
        if (!await ExistById(id))
            throw new NotFoundException(message: $"Role with id: {id} not exist", field: "RoleId");

        return true;
    }

    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(r => r.RoleId, id);
    }
}