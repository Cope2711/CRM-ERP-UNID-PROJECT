using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        this._authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginUserDto loginUserDto)
    {
        TokenDto? tokenDto = await this._authService.Login(loginUserDto);

        if (tokenDto == null)
        {
            return BadRequest("Some errors ocurre while login in :(");
        }

        return Ok(tokenDto);
    }
}