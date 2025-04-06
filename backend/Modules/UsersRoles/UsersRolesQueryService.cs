using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public class UsersRolesQueryService(
    IGenericService<UserRole> _genericService,
    IUsersRolesRepository _usersRolesRepository
    ) : IUsersRolesQueryService
{
    public async Task<UserRole?> GetById(Guid id)
    {
        return await _genericService.GetById(id);
    }
    
    public async Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId)
    {
        return await _usersRolesRepository.IsRoleAssignedToUserAsync(userId, roleId);
    }
    
    public async Task<double> GetMaxRolePriorityByUserId(Guid userId)
    {
        return await _usersRolesRepository.GetMaxUserRolePriority(userId);
    }
}