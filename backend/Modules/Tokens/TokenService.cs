using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRM_ERP_UNID.Data.Models;

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

    public string GenerateAccessToken(User user)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested GenerateAccessToken", AuthenticatedUserId);
        
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

        _logger.LogInformation("User with Id {AuthenticatedUserId} requested GenerateAccessToken and the access token was generated", AuthenticatedUserId);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(Guid userId)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested GenerateAndStoreRefreshTokenAsync", AuthenticatedUserId);
        
        string refreshToken = GenerateSecureToken();

        RefreshToken refreshTokenModel = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(2),
        };

        this._tokensRepository.AddRefreshToken(refreshTokenModel);
        await this._tokensRepository.SaveChangesAsync();

        _logger.LogInformation("User with Id {AuthenticatedUserId} requested GenerateAndStoreRefreshTokenAsync and the refresh token was generated", AuthenticatedUserId);
        
        return refreshTokenModel;
    }

    public async Task<RefreshToken?> GetRefreshTokenByRefreshToken(string refreshToken)
    {
        return await _genericService.GetFirstAsync(rt => rt.Token, refreshToken);
    }

    public async Task<RefreshToken> RevokeRefreshTokenByObject(RefreshToken refreshToken)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested RevokeRefreshTokenByObject for RefreshTokenId {TargetRefreshTokenId}", AuthenticatedUserId, refreshToken.RefreshTokenId);
        
        refreshToken.RevokedAt = DateTime.Now;
        await this._tokensRepository.SaveChangesAsync();
        
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested RevokeRefreshTokenByObject for RefreshTokenId {TargetRefreshTokenId} and the refresh token was revoked", AuthenticatedUserId, refreshToken.RefreshTokenId);
        
        return refreshToken;
    }

    public async Task RevokeRefreshsTokensByUserId(Guid userId)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested RevokeRefreshsTokensByUserId for UserId {TargetUserId}", AuthenticatedUserId, userId);
        await this._tokensRepository.RevokeTokensByUserIdAsync(userId);
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested RevokeRefreshsTokensByUserId for UserId {TargetUserId} and the refresh tokens were revoked", AuthenticatedUserId, userId);
    }

    private string GenerateSecureToken()
    {
        // Generar un buffer aleatorio seguro de 32 bytes
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        // Codificar el buffer en una cadena Base64
        return Convert.ToBase64String(randomNumber);
    }
}