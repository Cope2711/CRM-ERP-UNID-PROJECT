using System.Security.Claims;
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
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(User));
        
        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(User));
        
        
        GetAllResponseDto<User> getAllResponseDto = await _usersService.GetAll(getAllDto);

        if (getAllResponseDto == null)
        {
            return BadRequest("Some problems ocurred getting the users :(");
        }
        
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
        return Ok(await this._usersService.GetByEmail(email) != null);
    }
    
    [HttpGet("exist-user-by-username")]
    [PermissionAuthorize("View", "Users")]
    public async Task<ActionResult<bool>> ExistUserByUsername([FromQuery] string username)
    {
        return Ok(await this._usersService.GetByUserName(username) != null);
    }
    
    [HttpPut("deactivate")]
    [PermissionAuthorize("Deactivate_User")]
    public async Task<ActionResult<UserDto>> DeactivateUser([FromBody] DeactivateUserDto deactivateUserDto)
    {
        User user = await this._usersService.DeactivateUserAsync(deactivateUserDto.UserId);     
        return Ok(Mapper.UserToUserDto(user));
    }
    
    [HttpPut("change-password")]
    public async Task<ActionResult<UserDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        Guid userIdFromToken = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "");
        User user = await this._usersService.ChangePasswordAsync(userIdFromToken, changePasswordDto);
        return Ok(Mapper.UserToUserDto(user));
    }
    
}