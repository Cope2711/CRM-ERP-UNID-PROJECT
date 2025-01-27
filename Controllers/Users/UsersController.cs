using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Controllers;

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
    public async Task<ActionResult<User>> GetUserById([FromQuery] Guid id)
    {
        User user = await this._usersService.GetByIdThrowsNotFound(id);

        return Ok(user);
    }
    
    [HttpGet("get-by-username")]
    public async Task<ActionResult<User>> GetUserByUsername([FromQuery] string username)
    {
        User user = await this._usersService.GetByUserNameThrowsNotFound(username);

        return Ok(user);
    }
    
    [HttpPost("create")]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        User? user = await this._usersService.Create(createUserDto);

        if (user == null)
        {
            return BadRequest("Some problems ocurred creating the user :(");
        }

        return Ok(user);
    }

    [HttpGet("exist-user-by-email")]
    public async Task<ActionResult<bool>> ExistUserByEmail([FromQuery] string email)
    {
        return Ok(await this._usersService.GetByEmail(email) != null);
    }
    
    [HttpGet("exist-user-by-username")]
    public async Task<ActionResult<bool>> ExistUserByUsername([FromQuery] string username)
    {
        return Ok(await this._usersService.GetByUserName(username) != null);
    }
}