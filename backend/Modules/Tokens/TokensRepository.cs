using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface ITokensRepository
{
    Task RevokeTokensByUserIdAsync(Guid userId);
    
    /// <summary>
    /// Revokes all tokens associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose tokens are to be revoked.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    void AddRefreshToken(RefreshToken refreshToken);
    
    /// <summary>
    /// Adds a new refresh token to the repository.
    /// </summary>
    /// <param name="refreshToken">The refresh token to add.</param>
    Task SaveChangesAsync();
    
    /// <summary>
    /// Persists all changes made in the repository to the data store.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
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