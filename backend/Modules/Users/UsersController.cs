using System.Text.Json;
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
    [HttpGet("schema")]
    [PermissionAuthorize("View", "Users")]
    public IActionResult GetSchema([FromQuery] bool ignoreRequired = false)
    {
        return Ok(DtoSchemaHelper.GetDtoSchema(typeof(User), ignoreRequired));
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
    public async Task<ActionResult<UserDto>> Create([FromBody] User data)
    {
        User user = await _usersManagementService.Create(data);

        return Ok(user.ToDto());
    }
    
    [HttpPatch("update/{id}")]
    [PermissionAuthorize("Edit_Content", "Users")]
    public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] JsonElement data)
    {
        User user = await _usersManagementService.Update(id, data);
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
    [PermissionAuthorize("Deactivate", "Users")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> Deactivate(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> deactivateResponseDto =
            await _usersManagementService.Deactivate(idsDto);
        return Ok(deactivateResponseDto);
    }

    [HttpPatch("activate")]
    [PermissionAuthorize("Activate", "Users")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> Activate(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> activateResponseDto =
            await _usersManagementService.Activate(idsDto);
        return Ok(activateResponseDto);
    }

    [HttpPut("change-password")]
    public async Task<ActionResult<UserDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        User user = await _usersManagementService.ChangePassword(changePasswordDto);
        return Ok(user.ToDto());
    }
}