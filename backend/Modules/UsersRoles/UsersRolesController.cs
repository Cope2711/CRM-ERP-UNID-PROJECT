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
    
    [HttpPost("assign-role")]
    [PermissionAuthorize("Assign_Role")]
    public async Task<ActionResult<RolePermissionResourceDto>> AssignRole([FromBody] UserAndRoleDto userAndRoleDto)
    {
        UserRole userRole = await _usersRolesService.AssignRoleToUserAsync(userAndRoleDto);
        return Ok(Mapper.UserRoleToUserRoleDto(userRole));
    }
    
    [HttpDelete("revoke-role")]
    [PermissionAuthorize("Revoke_Role")]
    public async Task<ActionResult<UserRoleDto>> RevokeRole([FromBody] UserAndRoleDto userAndRoleDto)
    {
        UserRole userRole = await _usersRolesService.RevokeRoleToUserAsync(userAndRoleDto);
        return Ok(Mapper.UserRoleToUserRoleDto(userRole));
    }
    
    [HttpGet("get-all")]
    [PermissionAuthorize("View", "UsersRoles")]
    public async Task<ActionResult<GetAllResponseDto<UserRoleDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(UserRole));
        
        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(UserRole));
        
        GetAllResponseDto<UserRole> getAllResponseDto = await _usersRolesService.GetAllAsync(getAllDto);
        GetAllResponseDto<UserRoleDto> getAllResponseDtoDto = new GetAllResponseDto<UserRoleDto>();
        getAllResponseDtoDto.TotalItems = getAllResponseDto.TotalItems;
        getAllResponseDtoDto.TotalPages = getAllResponseDto.TotalPages;
        getAllResponseDtoDto.PageNumber = getAllResponseDto.PageNumber;
        getAllResponseDtoDto.PageSize = getAllResponseDto.PageSize;
        getAllResponseDtoDto.Data = getAllResponseDto.Data.Select(Mapper.UserRoleToUserRoleDto).ToList();
        
        return Ok(getAllResponseDtoDto);
    }
}
