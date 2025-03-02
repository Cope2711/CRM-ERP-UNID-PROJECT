using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class TokenService : ITokenService
{
    private readonly ITokensRepository _tokensRepository;
    private readonly IConfiguration _configuration;
    private readonly IGenericServie<RefreshToken> _genericService;
    private readonly ILogger<TokenService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Guid AuthenticatedUserId =>
        Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   Guid.Empty.ToString());

    public TokenService(IConfiguration configuration, ITokensRepository tokensRepository,
        IGenericServie<RefreshToken> genericService, ILogger<TokenService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        this._configuration = configuration;
        this._tokensRepository = tokensRepository;
        _genericService = genericService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task ValidateNumsOfDevices(Guid userId)
    {
        int numberOfDevices = await _tokensRepository.GetActiveDevicesCount(userId);
        if (numberOfDevices > 3)
            throw new UnauthorizedException(message: "Max number of devices reached", reason: "MaxNumberOfDevices");
    }
    
    public async Task<bool> IsNewDevice(Guid userId, string deviceId)
    {
        return await _tokensRepository.IsNewDevice(userId, deviceId);
    }
    
    public string GenerateAccessToken(User user)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        
        _logger.LogInformation("User with Id {authenticatedUserId} requested GenerateAccessToken", authenticatedUserId);

        var rolesIds = user.UserRoles.Select(ur => ur.Role.RoleId).ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserUserName),
            new Claim(ClaimTypes.Role, string.Join(",", rolesIds))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds
        );

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested GenerateAccessToken and the access token was generated",
            authenticatedUserId);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(Guid userId, string deviceId)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        
        _logger.LogInformation("User with Id {authenticatedUserId} requested GenerateAndStoreRefreshTokenAsync",
            authenticatedUserId);

        string refreshToken = GenerateSecureToken();

        RefreshToken refreshTokenModel = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            DeviceId = HasherHelper.HashDeviceIdForStorage(deviceId),
            ExpiresAt = DateTime.UtcNow.AddMinutes(2),
        };

        this._tokensRepository.AddRefreshToken(refreshTokenModel);
        await this._tokensRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested GenerateAndStoreRefreshTokenAsync and the refresh token was generated",
            authenticatedUserId);

        return refreshTokenModel;
    }

    public async Task<RefreshToken> GetRefreshTokenByRefreshTokenThrowsNotFound(string refreshToken)
    {
        RefreshToken? refreshTokenObject = await _genericService.GetFirstAsync(rt => rt.Token, refreshToken);

        if (refreshTokenObject == null)
            throw new NotFoundException("Refresh token not found", field: "RefreshToken");
        
        return refreshTokenObject;
    }

    public async Task<RefreshToken> RevokeRefreshTokenByObject(RefreshToken refreshToken)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRefreshTokenByObject for RefreshTokenId {TargetRefreshTokenId}",
            authenticatedUserId, refreshToken.RefreshTokenId);

        refreshToken.RevokedAt = DateTime.Now;
        await this._tokensRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRefreshTokenByObject for RefreshTokenId {TargetRefreshTokenId} and the refresh token was revoked",
            authenticatedUserId, refreshToken.RefreshTokenId);

        return refreshToken;
    }

    public async Task RevokeRefreshsTokensByUserId(Guid userId)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRefreshsTokensByUserId for UserId {TargetUserId}",
            authenticatedUserId, userId);
        await this._tokensRepository.RevokeTokensByUserIdAsync(userId);
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