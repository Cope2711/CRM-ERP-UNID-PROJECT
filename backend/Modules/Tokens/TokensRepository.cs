using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface ITokensRepository
{
    Task RevokeTokensByUserIdAsync(Guid userId);
    void AddRefreshToken(RefreshToken refreshToken);
    Task SaveChangesAsync();
}

public class TokensRepository : ITokensRepository
{
    private readonly AppDbContext _context;

    public TokensRepository(AppDbContext context)
    {
        this._context = context;
    }
    
    public async Task RevokeTokensByUserIdAsync(Guid userId)
    {
        var tokens = _context.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ToList();

        if (tokens.Any())
        {
            foreach (var token in tokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }
    }
    
    public void AddRefreshToken(RefreshToken refreshToken)
    {
        this._context.RefreshTokens.Add(refreshToken);
    }
    
    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}