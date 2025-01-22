using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Controllers.Users;

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
    public async Task<ActionResult<User>> GetUser([FromQuery] Guid id)
    {
        User? user = await this._usersService.GetById(id);

        if (user == null)
        {
            return NotFound(user);
        }

        return Ok(user);
    }
    
    [HttpPost("create")]
    public async Task<ActionResult<User>> GetUser([FromBody] CreateUserDto createUserDto)
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