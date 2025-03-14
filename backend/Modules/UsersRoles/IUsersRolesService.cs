using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesService
{
    Task<UserRole> GetByUserIdAndRoleIdThrowsNotFoundAsync(Guid userId, Guid roleId);
    Task<UserRole?> GetByUserIdAndRoleId(Guid userId, Guid roleId);
    Task<ResponsesDto<UserAndRoleResponseStatusDto>> AssignRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto);
    Task<ResponsesDto<UserAndRoleResponseStatusDto>> RevokeRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto);
    Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId);
    Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto);
}