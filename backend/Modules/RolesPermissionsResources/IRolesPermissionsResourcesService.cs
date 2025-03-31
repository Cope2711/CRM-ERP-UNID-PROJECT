using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IRolesPermissionsResourcesService
{
    Task<bool> ArePermissionNameResourceNameAssignedToRoleIdAsync(Guid roleId, string permissionName,
        string? resourceName = null);

    Task<bool> ArePermissionIdResourceIdAssignedToRoleIdAsync(Guid roleId, Guid permissionId, Guid? resourceId = null);

    Task<ResponsesDto<RolePermissionResourceResponseStatusDto>> AssignPermissionsToRolesAsync(
        PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto);

    Task<ResponsesDto<IdResponseStatusDto>> RevokePermissionsToRolesAsync(
        IdsDto idsDto);

    Task<GetAllResponseDto<RolePermissionResource>> GetAllAsync(GetAllDto getAllDto);
}