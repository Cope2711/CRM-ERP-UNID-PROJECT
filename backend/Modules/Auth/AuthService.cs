using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public interface IAuthService
{
    Task<TokenDto> Login(LoginUserDto loginUserDto);
    Task<TokenDto> RefreshTokenAsync(string refreshTokenString);
    Task<RefreshToken?> Logout(string refreshTokenString);
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

    public async Task<TokenDto> RefreshTokenAsync(string refreshTokenString)
    {
        // 1. Get the refreshToken object 
        RefreshToken? refreshToken = await this._tokenService.GetRefreshTokenByRefreshToken(refreshTokenString);

        this.ValidateRefreshToken(refreshToken);

        // 2. Get the user
        User user = await this._usersService.GetByIdThrowsNotFoundAsync(refreshToken.UserId);

        // 3. Create the new Token Data
        TokenDto newTokenDto = new TokenDto
        {
            Token = this._tokenService.GenerateAccessToken(user),
            RefreshToken = refreshToken.Token
        };

        return newTokenDto;
    }

    public async Task<TokenDto> Login(LoginUserDto loginUserDto)
    {
        // 1. Exist User
        User? user = await this._usersService.GetByUserNameThrowsNotFound(loginUserDto.UserUserName);

        // 2. Correct password?
        if (!PasswordHelper.VerifyPassword(loginUserDto.UserPassword, user.UserPassword))
        {
            throw new UnauthorizedException(message: "The password is incorrect.", reason: "IncorrectPassword");
        }

        return new TokenDto
        {
            Token = this._tokenService.GenerateAccessToken(user),
            RefreshToken = (await this._tokenService.GenerateAndStoreRefreshTokenAsync(user.UserId)).Token
        };
    }

    public async Task<RefreshToken?> Logout(string refreshTokenString)
    {
        // 1. Get the refresh token
        RefreshToken? refreshToken = await this._tokenService.GetRefreshTokenByRefreshToken(refreshTokenString);

        this.ValidateRefreshToken(refreshToken);

        // 2. Revoke refresh token and return
        await this._tokenService.RevokeRefreshTokenByObject(refreshToken);

        return refreshToken;
    }

    private void ValidateRefreshToken(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
        {
            throw new UnauthorizedException(message: "InvalidToken", reason: "Refresh token is null.");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedException(message: "Refresh token has expired.", reason: "ExpiredToken");
        }

        if (refreshToken.RevokedAt != null)
        {
            throw new UnauthorizedException(message: "Refresh token has been revoked.", reason: "RevokedToken");
        }
    }
}