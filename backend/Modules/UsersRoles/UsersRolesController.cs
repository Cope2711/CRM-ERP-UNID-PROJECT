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
public class UsersRolesController : ControllerBase
{
    private readonly IUsersRolesService _usersRolesService;
    
    public UsersRolesController(IUsersRolesService usersRolesService)
    {
        _usersRolesService = usersRolesService;
    }
    
    
    [HttpPost("assign-roles")]
    [PermissionAuthorize("Assign", "UsersRoles")] 
    public async Task<ActionResult<ResponsesDto<UserAndRoleResponseStatusDto>>> AssignRoles([FromBody] UsersAndRolesDtos usersAndRolesDto)
    {
        ResponsesDto<UserAndRoleResponseStatusDto> usersAndRolesResponsesDto = await _usersRolesService.AssignRolesToUsersAsync(usersAndRolesDto);
        return Ok(usersAndRolesResponsesDto);
    }
    
    [HttpDelete("revoke-roles")]
    [PermissionAuthorize("Revoke", "UsersRoles")]
    public async Task<ActionResult<ResponsesDto<UserAndRoleResponseStatusDto>>> RevokeRoles([FromBody] UsersAndRolesDtos usersAndRolesDto)
    {
        ResponsesDto<UserAndRoleResponseStatusDto> usersAndRolesResponsesDto = await _usersRolesService.RevokeRolesToUsersAsync(usersAndRolesDto);
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
        
        GetAllResponseDto<UserRole> getAllResponseDto = await _usersRolesService.GetAllAsync(getAllDto);
        
        return Ok(getAllResponseDto);
    }
}
