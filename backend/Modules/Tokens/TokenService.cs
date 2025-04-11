using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class TokenService(
    IConfiguration _configuration,
    ITokensRepository _tokensRepository,
    IGenericService<RefreshToken> _genericService,
    ILogger<TokenService> _logger,
    IHttpContextAccessor _httpContextAccessor
) : ITokenService
{
    public async Task ValidateNumsOfDevices(Guid userId)
    {
        int numberOfDevices = await _tokensRepository.GetActiveDevicesCount(userId);
        if (numberOfDevices > 3)
            throw new UnauthorizedException(message: "Max number of devices reached", reason: Reasons.MaxNumberOfDevices);
    }
    
    public async Task<bool> IsNewDevice(Guid userId, string deviceId)
    {
        return await _tokensRepository.IsNewDevice(userId, deviceId);
    }
    
    public string GenerateAccessToken(User user)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation("User with Id {authenticatedUserId} requested GenerateAccessToken", authenticatedUserId);

        var rolesIds = user.UserRoles.Select(ur => ur.Role.RoleId).ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserUserName),
            new Claim(ClaimTypes.Role, string.Join(",", rolesIds)),
            new Claim("MaxRolePriority", user.ToUserRolesPriority().Max().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), 
            signingCredentials: creds
        );

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested GenerateAccessToken and the access token was generated",
            authenticatedUserId);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(Guid userId, string deviceId)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation("User with Id {authenticatedUserId} requested GenerateAndStoreRefreshTokenAsync",
            authenticatedUserId);

        string refreshToken = GenerateSecureToken();

        RefreshToken refreshTokenModel = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            DeviceId = HasherHelper.HashDeviceIdForStorage(deviceId),
            ExpiresAt = DateTime.UtcNow.AddHours(12),
        };

        _tokensRepository.AddRefreshToken(refreshTokenModel);
        await _tokensRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested GenerateAndStoreRefreshTokenAsync and the refresh token was generated",
            authenticatedUserId);

        return refreshTokenModel;
    }

    public async Task<RefreshToken> GetRefreshTokenByRefreshTokenThrowsNotFound(string refreshToken)
    {
        RefreshToken? refreshTokenObject = await _genericService.GetFirstAsync(rt => rt.Token, refreshToken);

        if (refreshTokenObject == null)
            throw new NotFoundException("Refresh token not found", field: Fields.RefreshTokens.RefreshTokenField);
        
        return refreshTokenObject;
    }

    public async Task<RefreshToken> RevokeRefreshTokenByObject(RefreshToken refreshToken)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRefreshTokenByObject for RefreshTokenId {TargetRefreshTokenId}",
            authenticatedUserId, refreshToken.RefreshTokenId);

        refreshToken.RevokedAt = DateTime.Now;
        await _tokensRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRefreshTokenByObject for RefreshTokenId {TargetRefreshTokenId} and the refresh token was revoked",
            authenticatedUserId, refreshToken.RefreshTokenId);

        return refreshToken;
    }

    public async Task RevokeRefreshTokensByUserId(Guid userId)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRefreshsTokensByUserId for UserId {TargetUserId}",
            authenticatedUserId, userId);
        await _tokensRepository.RevokeTokensByUserIdAsync(userId);
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRefreshsTokensByUserId for UserId {TargetUserId} and the refresh tokens were revoked",
            authenticatedUserId, userId);
    }

    private string GenerateSecureToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
}