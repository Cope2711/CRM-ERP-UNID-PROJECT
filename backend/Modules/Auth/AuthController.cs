﻿using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]



[Route("api/auth")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IPasswordResetService _passwordResetService;

    public AuthController(IAuthService authService, IPasswordResetService passwordResetService)
    {
        this._authService = authService;
        this._passwordResetService = passwordResetService;
        
    }

    [AllowAnonymous]
    [HttpPost("request-reset")]
    public async Task<ActionResult> RequestResetAsync([FromBody] RequestPasswordResetDto request)
    {
        var result = await _passwordResetService.RequestPasswordResetAsync(request.Email);
        if(!result)
            return BadRequest("The request could not be processed.d.");
        return Ok("It's ah sent correctly");
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        var result = await _passwordResetService.ResetPasswordAsync(request);
        if (!result)
            return BadRequest("Invalid or expired token.");
        return Ok("Password reset successfully.");
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
        TokenDto? tokenDto = await this._authService.RefreshTokenAsync(refreshTokenEntryDto);

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