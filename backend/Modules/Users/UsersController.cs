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
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    
    public UsersController(IUsersService usersService)
    {
        this._usersService = usersService;
    }

    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Users")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        User user = await this._usersService.UpdateAsync(updateUserDto);
        return Ok(Mapper.UserToUserDto(user));
    }

    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<UserDto>> GetUserById([FromQuery] Guid id)
    {
        User user = await this._usersService.GetByIdThrowsNotFoundAsync(id);

        return Ok(Mapper.UserToUserDto(user));
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<GetAllResponseDto<UserDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(User));
        
        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(User));
        
        
        GetAllResponseDto<User> getAllResponseDto = await _usersService.GetAll(getAllDto);
        
        GetAllResponseDto<UserDto> getAllResponseDtoDto = new GetAllResponseDto<UserDto>();
        getAllResponseDtoDto.TotalItems = getAllResponseDto.TotalItems;
        getAllResponseDtoDto.TotalPages = getAllResponseDto.TotalPages;
        getAllResponseDtoDto.PageNumber = getAllResponseDto.PageNumber;
        getAllResponseDtoDto.PageSize = getAllResponseDto.PageSize;
        getAllResponseDtoDto.Data = getAllResponseDto.Data.Select(u => Mapper.UserToUserDto(u)).ToList();

        return Ok(getAllResponseDtoDto);
    }
    
    [HttpGet("get-by-username")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<UserDto>> GetUserByUsername([FromQuery] string username)
    {
        User user = await this._usersService.GetByUserNameThrowsNotFound(username);

        return Ok(Mapper.UserToUserDto(user));
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Users")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        User? user = await this._usersService.Create(createUserDto);

        if (user == null)
        {
            return BadRequest("Some problems ocurred creating the user :(");
        }

        return Ok(Mapper.UserToUserDto(user));
    }

    [HttpGet("exist-user-by-email")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<bool>> ExistUserByEmail([FromQuery] string email)
    {
        return Ok(await this._usersService.ExistByEmail(email));
    }
    
    [HttpGet("exist-user-by-username")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<bool>> ExistUserByUsername([FromQuery] string username)
    {
        return Ok(await this._usersService.GetByUserName(username) != null);
    }
    
    [HttpPatch("deactivate")]
    [PermissionAuthorize("Deactivate_User")]
    public async Task<ActionResult<ResponsesDto<UserResponseStatusDto>>> DeactivateUser([FromBody] UsersIdsDto usersIdsDto)
    {
        ResponsesDto<UserResponseStatusDto> deactivateUsersResponseDto = await this._usersService.DeactivateUsersAsync(usersIdsDto);     
        return Ok(deactivateUsersResponseDto);
    }
    
    [HttpPatch("activate")]
    [PermissionAuthorize("Activate_User")]
    public async Task<ActionResult<ResponsesDto<UserResponseStatusDto>>> ActivateUser([FromBody] UsersIdsDto usersIdsDto)
    {
        ResponsesDto<UserResponseStatusDto> activateUsersResponseDto = await this._usersService.ActivateUsersAsync(usersIdsDto);
        return Ok(activateUsersResponseDto);
    }
    
    [HttpPut("change-password")]
    public async Task<ActionResult<UserDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        User user = await this._usersService.ChangePasswordAsync(changePasswordDto);
        return Ok(Mapper.UserToUserDto(user));
    }
    
}