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

public class AuthService(
    IUsersService _usersService,
    ITokenService _tokenService,
    IMailService _mailService,
    ILogger<AuthService> _logger
) : IAuthService
{
    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenEntryDto refreshTokenEntryDto)
    {
        // Get the refreshToken object 
        RefreshToken refreshToken = await _tokenService.GetRefreshTokenByRefreshTokenThrowsNotFound(refreshTokenEntryDto.RefreshToken);

        // Is the same deviceId?
        if (HasherHelper.VerifyDeviceId(refreshToken.DeviceId, refreshTokenEntryDto.DeviceId) == false)
        {
            await _tokenService.RevokeRefreshTokensByUserId(refreshToken.UserId);
            throw new UnauthorizedException(message: "Device not authorizated to refresh the token.", reason: "WrongDeviceId");
        }
        
        ValidateRefreshToken(refreshToken);

        // Get the user
        User user = await _usersService.GetByIdThrowsNotFoundAsync(refreshToken.UserId);

        // Create the new Token Data
        TokenDto newTokenDto = new TokenDto
        {
            Token = _tokenService.GenerateAccessToken(user),
            RefreshToken = refreshToken.Token
        };
        
        return newTokenDto;
    }

    public async Task<TokenDto> Login(LoginUserDto loginUserDto)
    {
        _logger.LogInformation("User with UserName {UserUserName} requested Login", loginUserDto.UserUserName);
        
        // Exist User
        User? user = await _usersService.GetByUserNameThrowsNotFound(loginUserDto.UserUserName);

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
            Token = _tokenService.GenerateAccessToken(user),
            RefreshToken = (await _tokenService.GenerateAndStoreRefreshTokenAsync(user.UserId, loginUserDto.DeviceId)).Token
        };
    }

    public async Task<RefreshToken?> Logout(string refreshTokenString)
    {
        // 1. Get the refresh token
        RefreshToken? refreshToken = await _tokenService.GetRefreshTokenByRefreshTokenThrowsNotFound(refreshTokenString);

        ValidateRefreshToken(refreshToken);

        // 2. Revoke refresh token and return
        await _tokenService.RevokeRefreshTokenByObject(refreshToken);

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