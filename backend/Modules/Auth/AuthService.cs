using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class AuthService(
    IUsersQueryService _usersQueryService,
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
        if (HasherHelper.VerifyDeviceId(refreshToken.deviceId, refreshTokenEntryDto.DeviceId) == false)
        {
            await _tokenService.RevokeRefreshTokensByUserId(refreshToken.userId);
            throw new UnauthorizedException(message: "Device not authorizated to refresh the token.", reason: Reasons.WrongDeviceId);
        }
        
        ValidateRefreshToken(refreshToken);

        // Get the user
        User user = await _usersQueryService.GetByIdThrowsNotFoundAsync(refreshToken.userId);

        // Create the new Token Data
        TokenDto newTokenDto = new TokenDto
        {
            token = _tokenService.GenerateAccessToken(user),
            refreshToken = refreshToken.token,
            User = user.ToDto()
        };
        
        return newTokenDto;
    }

    public async Task<TokenDto> Login(LoginUserDto loginUserDto)
    {
        _logger.LogInformation("User with UserName {UserUserName} requested Login", loginUserDto.UserUserName);
        
        // Exist User
        User? user = await _usersQueryService.GetByUserNameThrowsNotFound(loginUserDto.UserUserName);

        // Correct password?
        if (!HasherHelper.VerifyHash(loginUserDto.UserPassword, user.password))
        {
            _logger.LogError("User with UserName {UserUserName} requested Login but the password is incorrect", loginUserDto.UserUserName);
            throw new UnauthorizedException(message: "The password is incorrect.", reason: Reasons.WrongPassword, field: Fields.Users.password);
        }   

        await _tokenService.ValidateNumsOfDevices(user.id);
        
        if (await _tokenService.IsNewDevice(user.id, loginUserDto.DeviceId))
        {
            _logger.LogInformation("User with UserName {UserUserName} requested Login and the device is New", loginUserDto.UserUserName);
            await _mailService.SendNewDeviceLoggedInMailAsync();
        }
        
        _logger.LogInformation("User with UserName {UserUserName} requested Login and the user was logged in", loginUserDto.UserUserName);
        
        return new TokenDto
        {
            token = _tokenService.GenerateAccessToken(user),
            refreshToken = (await _tokenService.GenerateAndStoreRefreshTokenAsync(user.id, loginUserDto.DeviceId)).token,
            User = user.ToDto()
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
        if (refreshToken.expiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedException(message: "Refresh token has expired.", reason: Reasons.ExpiredToken);
        }

        if (refreshToken.revokedAt != null)
        {
            throw new UnauthorizedException(message: "Refresh token has been revoked.", reason: Reasons.RevokedToken);
        }
    }
    
}