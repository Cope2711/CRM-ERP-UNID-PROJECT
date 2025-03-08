﻿using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Controllers;

[ApiController]
[Route("api/auth")]
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

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken([FromBody] RefreshTokenEntryDto refreshTokenEntryDto)
    {
        TokenDto? tokenDto = await this._authService.RefreshTokenAsync(refreshTokenEntryDto.RefreshToken);

        if (tokenDto == null)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token." }); 
        }

        return Ok(tokenDto);
    }
    
    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenDto>> Logout([FromBody] RefreshTokenEntryDto refreshTokenEntryDto)
    {
        RefreshToken? refreshToken = await this._authService.Logout(refreshTokenEntryDto.RefreshToken);
        
        if (refreshToken == null)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }

        return Ok(new TokenDto
        {
            Token = null,
            RefreshToken = refreshToken.Token
        });
    }
}