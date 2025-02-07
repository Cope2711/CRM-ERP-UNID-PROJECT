using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface ITokensRepository
{
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

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        this._context.RefreshTokens.Add(refreshToken);
    }
    
    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}