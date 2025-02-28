using System.Security.Claims;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public interface IAuthService
{
    Task<TokenDto> Login(LoginUserDto loginUserDto);
    Task<TokenDto> RefreshTokenAsync(RefreshTokenEntryDto refreshTokenEntryDto);
    Task<RefreshToken?> Logout(string refreshTokenString);
}

public class AuthService : IAuthService
{
    private readonly IUsersService _usersService;
    private readonly ITokenService _tokenService;
    private readonly IMailService _mailService;
    private readonly ILogger<AuthService> _logger;
    
    public AuthService(IUsersService usersService, ITokenService tokenService, ILogger<AuthService> logger, IMailService mailService,IUsersRepository usersRepository)
    {
        _usersService = usersService;
        _tokenService = tokenService;
        _mailService = mailService;
        _logger = logger;
    }

    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenEntryDto refreshTokenEntryDto)
    {
        // Get the refreshToken object 
        RefreshToken refreshToken = await this._tokenService.GetRefreshTokenByRefreshTokenThrowsNotFound(refreshTokenEntryDto.RefreshToken);

        // Is the same deviceId?
        if (HasherHelper.VerifyDeviceId(refreshToken.DeviceId, refreshTokenEntryDto.DeviceId) == false)
        {
            await _tokenService.RevokeRefreshsTokensByUserId(refreshToken.UserId);
            throw new UnauthorizedException(message: "Device not authorizated to refresh the token.", reason: "WrongDeviceId");
        }
        
        this.ValidateRefreshToken(refreshToken);

        // Get the user
        User user = await this._usersService.GetByIdThrowsNotFoundAsync(refreshToken.UserId);

        // Create the new Token Data
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
        
        // Exist User
        User? user = await this._usersService.GetByUserNameThrowsNotFound(loginUserDto.UserUserName);

        // Correct password?
        if (!HasherHelper.VerifyHash(loginUserDto.UserPassword, user.UserPassword))
        {
            _logger.LogError("User with UserName {UserUserName} requested Login but the password is incorrect", loginUserDto.UserUserName);
            throw new UnauthorizedException(message: "The password is incorrect.", reason: "IncorrectPassword");
        }

        await _tokenService.ValidateNumsOfDevices(user.UserId);
        
        if (await _tokenService.IsNewDevice(user.UserId, loginUserDto.DeviceId))
        {
            _logger.LogInformation("User with UserName {UserUserName} requested Login and the device is New", loginUserDto.UserUserName);
            await _mailService.SendNewDeviceLoggedInMailAsync();
        }
        
        _logger.LogInformation("User with UserName {UserUserName} requested Login and the user was logged in", loginUserDto.UserUserName);
        
        return new TokenDto
        {
            Token = this._tokenService.GenerateAccessToken(user),
            RefreshToken = (await this._tokenService.GenerateAndStoreRefreshTokenAsync(user.UserId, loginUserDto.DeviceId)).Token
        };
    }

    public async Task<RefreshToken?> Logout(string refreshTokenString)
    {
        // 1. Get the refresh token
        RefreshToken? refreshToken = await this._tokenService.GetRefreshTokenByRefreshTokenThrowsNotFound(refreshTokenString);

        this.ValidateRefreshToken(refreshToken);

        // 2. Revoke refresh token and return
        await this._tokenService.RevokeRefreshTokenByObject(refreshToken);

        return refreshToken;
    }

    private void ValidateRefreshToken(RefreshToken? refreshToken)
    {
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