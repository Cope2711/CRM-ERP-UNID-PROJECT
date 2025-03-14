using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/users")]
public class UsersController(
    IUsersManagementService _usersManagementService,
    IUsersQueryService _usersQueryService
) : ControllerBase
{
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Users")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        User user = await _usersManagementService.UpdateAsync(updateUserDto);
        return Ok(user.ToDto());
    }

    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<UserDto>> GetUserById([FromQuery] Guid id)
    {
        User user = await _usersQueryService.GetByIdThrowsNotFoundAsync(id);

        return Ok(user.ToDto());
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<GetAllResponseDto<User>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(User));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(User));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(User));

        GetAllResponseDto<User> getAllResponseDto = await _usersQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }

    [HttpGet("get-by-username")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<UserDto>> GetUserByUsername([FromQuery] string username)
    {
        User user = await _usersQueryService.GetByUserNameThrowsNotFound(username);

        return Ok(user.ToDto());
    }

    [HttpPost("create")]
    [PermissionAuthorize("Create", "Users")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        User? user = await _usersManagementService.Create(createUserDto);

        if (user == null)
        {
            return BadRequest("Some problems ocurred creating the user :(");
        }

        return Ok(user.ToDto());
    }

    [HttpGet("exist-user-by-email")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<bool>> ExistUserByEmail([FromQuery] string email)
    {
        return Ok(await _usersQueryService.ExistByEmail(email));
    }

    [HttpGet("exist-user-by-username")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<bool>> ExistUserByUsername([FromQuery] string username)
    {
        return Ok(await _usersQueryService.GetByUserName(username) != null);
    }

    [HttpPatch("deactivate")]
    [PermissionAuthorize("Deactivate_User")]
    public async Task<ActionResult<ResponsesDto<UserResponseStatusDto>>> DeactivateUser(
        [FromBody] UsersIdsDto usersIdsDto)
    {
        ResponsesDto<UserResponseStatusDto> deactivateUsersResponseDto =
            await _usersManagementService.DeactivateUsersAsync(usersIdsDto);
        return Ok(deactivateUsersResponseDto);
    }

    [HttpPatch("activate")]
    [PermissionAuthorize("Activate_User")]
    public async Task<ActionResult<ResponsesDto<UserResponseStatusDto>>> ActivateUser(
        [FromBody] UsersIdsDto usersIdsDto)
    {
        ResponsesDto<UserResponseStatusDto> activateUsersResponseDto =
            await _usersManagementService.ActivateUsersAsync(usersIdsDto);
        return Ok(activateUsersResponseDto);
    }

    [HttpPut("change-password")]
    public async Task<ActionResult<UserDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        User user = await _usersManagementService.ChangePasswordAsync(changePasswordDto);
        return Ok(user.ToDto());
    }
}