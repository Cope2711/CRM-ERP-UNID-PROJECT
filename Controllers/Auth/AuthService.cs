using System.Security.Claims;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Controllers;

public interface IAuthService
{
    Task<TokenDto> Login(LoginUserDto loginUserDto);
}

public class AuthService : IAuthService
{
    private readonly IUsersService _usersService;
    private readonly ITokenService _tokenService;

    public AuthService(IUsersService usersService, ITokenService tokenService)
    {
        this._usersService = usersService;
        this._tokenService = tokenService;
    }

    public async Task<TokenDto> Login(LoginUserDto loginUserDto)
    {
        // 1. Exist User
        User? user = await this._usersService.GetByUserName(loginUserDto.UserUserName);

        if (user == null)
        {
            return null;
        }

        // 2. Correct password?
        if (!PasswordHelper.VerifyPassword(loginUserDto.UserPassword, user.UserPassword))
        {
            return null;
        }

        return new TokenDto
        {
            Token = this._tokenService.GenerateAccessToken(claims: new[]
            {
                new Claim(ClaimTypes.Name, user.UserUserName),
                new Claim(ClaimTypes.Role, "User")
            }),
            RefreshToken = this._tokenService.GenerateRefreshToken()
        };
    }
}