using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesQueryService
{
    Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId);
    Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto);
    Task<UserRole?> GetById(Guid id);
    Task<double[]> GetUserRolesPriorityByUserId(Guid userId);
}