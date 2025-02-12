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
    private readonly IRolesPermissionsResourcesService _rolesPermissionsResourcesService;

    public TokenService(IConfiguration configuration, ITokensRepository tokensRepository, IGenericServie<RefreshToken> genericService, IRolesPermissionsResourcesService rolesPermissionsResourcesService)
    {
        this._configuration = configuration;
        this._tokensRepository = tokensRepository;
        _genericService = genericService;
        _rolesPermissionsResourcesService = rolesPermissionsResourcesService;
    }

    public string GenerateAccessToken(User user)
    {
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

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(Guid userId)
    {
        string refreshToken = GenerateSecureToken();

        RefreshToken refreshTokenModel = new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(2),
        };

        this._tokensRepository.AddRefreshToken(refreshTokenModel);
        await this._tokensRepository.SaveChangesAsync();

        return refreshTokenModel;
    }

    public async Task<RefreshToken?> GetRefreshTokenByRefreshToken(string refreshToken)
    {
       return await _genericService.GetFirstAsync(rt => rt.Token, refreshToken);
    }

    public async Task<RefreshToken> RevokeRefreshTokenByObject(RefreshToken refreshToken)
    {
        refreshToken.RevokedAt = DateTime.Now;
        await this._tokensRepository.SaveChangesAsync();
        return refreshToken;
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