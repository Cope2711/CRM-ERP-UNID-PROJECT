using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers;

public interface ITokensRepository
{
    void AddRefreshToken(RefreshToken refreshToken);
    Task SaveChangesAsync();
    Task<RefreshToken?> GetRefreshTokenByRefreshToken(string refreshToken);
}

public class TokensRepository : ITokensRepository
{
    private readonly AppDbContext _context;

    public TokensRepository(AppDbContext context)
    {
        this._context = context;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        this._context.RefreshTokens.Add(refreshToken);
    }
    
    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenByRefreshToken(string refreshToken)
    {
        return await this._context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }
}