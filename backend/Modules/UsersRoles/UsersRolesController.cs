using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Route("api/users-roles")]
[Authorize]
public class UsersRolesController(
    IUsersRolesManagementService _usersRolesManagementService,
    IUsersRolesQueryService _usersRolesQueryService
) : ControllerBase
{
    [HttpPost("assign-roles")]
    [PermissionAuthorize("Assign", "UsersRoles")] 
    public async Task<ActionResult<ResponsesDto<UserAndRoleResponseStatusDto>>> AssignRoles([FromBody] UsersAndRolesDtos usersAndRolesDto)
    {
        ResponsesDto<UserAndRoleResponseStatusDto> usersAndRolesResponsesDto = await _usersRolesManagementService.AssignRolesToUsersAsync(usersAndRolesDto);
        return Ok(usersAndRolesResponsesDto);
    }
    
    [HttpDelete("revoke-roles")]
    [PermissionAuthorize("Revoke", "UsersRoles")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> RevokeRoles([FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> usersAndRolesResponsesDto = await _usersRolesManagementService.RevokeRolesToUsersAsync(idsDto);
        return Ok(usersAndRolesResponsesDto);
    }
    
    [HttpGet("get-all")]
    [PermissionAuthorize("View", "UsersRoles")]
    public async Task<ActionResult<GetAllResponseDto<UserRole>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(UserRole));
        
        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(UserRole));
        
        GetAllResponseDto<UserRole> getAllResponseDto = await _usersRolesQueryService.GetAllAsync(getAllDto);
        
        return Ok(getAllResponseDto);
    }
}
