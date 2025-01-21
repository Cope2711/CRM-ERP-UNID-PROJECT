using CRM_ERP_UNID.Data.Models;
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
}