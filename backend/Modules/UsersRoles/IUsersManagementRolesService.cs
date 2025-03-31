using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesManagementService
{
    Task<ResponsesDto<UserAndRoleResponseStatusDto>> AssignRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto); 
    Task<ResponsesDto<IdResponseStatusDto>> RevokeRolesToUsersAsync(IdsDto idsDto);
}