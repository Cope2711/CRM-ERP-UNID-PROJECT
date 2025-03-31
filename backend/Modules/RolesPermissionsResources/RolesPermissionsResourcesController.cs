using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Route("api/roles-permissions")]
[Authorize]
public class RolesPermissionsResourcesController(
    IRolesPermissionsResourcesService _rolesPermissionsResourcesService
) : ControllerBase
{
    [HttpPost("assign-permissions")]
    [PermissionAuthorize("Assign", "RolesPermissionsResources")]
    public async Task<ActionResult<ResponsesDto<RolePermissionResourceResponseStatusDto>>> AssignPermissionsToRoles(
        [FromBody] PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto)
    {
        ResponsesDto<RolePermissionResourceResponseStatusDto> responsesDto =
            await _rolesPermissionsResourcesService.AssignPermissionsToRolesAsync(permissionsResourcesAndRolesIdsDto);
        return Ok(responsesDto);
    }

    [HttpDelete("revoke-permissions")]
    [PermissionAuthorize("Revoke", "RolesPermissionsResources")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> RevokePermissionsToRoles(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> responsesDto =
            await _rolesPermissionsResourcesService.RevokePermissionsToRolesAsync(idsDto);
        return Ok(responsesDto);
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "RolesPermissionsResources")]
    public async Task<ActionResult<GetAllResponseDto<RolePermissionResource>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(RolePermissionResource));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(RolePermissionResource));

        GetAllResponseDto<RolePermissionResource> getAllResponseDto =
            await _rolesPermissionsResourcesService.GetAllAsync(getAllDto);

        return Ok(getAllResponseDto);
    }
}