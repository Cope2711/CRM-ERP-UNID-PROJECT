using System.Security.Claims;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public interface IAuthService
{
    Task<TokenDto> Login(LoginUserDto loginUserDto);
    
    /// <summary>
    /// Authenticates a user and generates access and refresh tokens.
    /// </summary>
    /// <param name="LoginUserDto" >The DTO containing the user's login credentials.</param>
    /// <returns>A DTO containing the generated access and refresh tokens.</returns>
    /// <exception cref="UnauthorizedException">Thrown when the login credentials are invalid.</exception>
    Task<TokenDto> RefreshTokenAsync(string refreshTokenString);
    
    /// <summary>
    /// Generates a new access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshTokenString">The refresh token string.</param>
    /// <returns>A DTO containing the new access token and refresh token.</returns>
    /// <exception cref="UnauthorizedException">Thrown when the refresh token is invalid or expired.</exception>
    Task<RefreshToken?> Logout(string refreshTokenString);
    
    /// <summary>
    /// Logs out the user by revoking the provided refresh token.
    /// </summary>
    /// <param name="refreshTokenString">The refresh token string to be revoked.</param>
    /// <returns>The revoked refresh token if the operation is successful; otherwise, null.</returns>
}
    
public class AuthService : IAuthService
{
    private readonly IUsersService _usersService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;
    
    public AuthService(IUsersService usersService, ITokenService tokenService, ILogger<AuthService> logger)
    {
        this._usersService = usersService;
        this._tokenService = tokenService;
        _logger = logger;
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
        _logger.LogInformation("User with UserName {UserUserName} requested Login", loginUserDto.UserUserName);
        
        // 1. Exist User
        User? user = await this._usersService.GetByUserNameThrowsNotFound(loginUserDto.UserUserName);

        // 2. Correct password?
        if (!PasswordHelper.VerifyPassword(loginUserDto.UserPassword, user.UserPassword))
        {
            _logger.LogError("User with UserName {UserUserName} requested Login but the password is incorrect", loginUserDto.UserUserName);
            throw new UnauthorizedException(message: "The password is incorrect.", reason: "IncorrectPassword");
        }

        _logger.LogInformation("User with UserName {UserUserName} requested Login and the user was logged in", loginUserDto.UserUserName);
        
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