using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface ITokensRepository
{
    Task<int> GetActiveDevicesCount(Guid userId);
    Task RevokeTokensByUserIdAsync(Guid userId);
    void AddRefreshToken(RefreshToken refreshToken);
    Task SaveChangesAsync();
    Task<bool> IsNewDevice(Guid userId, string deviceId);
}

public class TokensRepository : ITokensRepository
{
    private readonly AppDbContext _context;

    public TokensRepository(AppDbContext context)
    {
        this._context = context;
    }

    public async Task<int> GetActiveDevicesCount(Guid userId)
    {
        return await _context.RefreshTokens
            .Where(rt => rt.userId == userId && rt.revokedAt == null && rt.expiresAt > DateTime.UtcNow)
            .Select(rt => rt.deviceId)
            .Distinct()
            .CountAsync();
    }

    public async Task<bool> IsNewDevice(Guid userId, string deviceId)
    {
        return await _context.RefreshTokens.AnyAsync(rt => rt.userId == userId && rt.deviceId == deviceId);
    }

    public async Task RevokeTokensByUserIdAsync(Guid userId)
    {
        var tokens = _context.RefreshTokens
            .Where(t => t.userId == userId && t.revokedAt == null)
            .ToList();

        if (tokens.Any())
        {
            foreach (var token in tokens)
            {
                token.revokedAt = DateTime.UtcNow;
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