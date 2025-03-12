using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IPasswordResetRepository
{
    Task<PasswordRecoveryToken?> GetByTokenAndEmailThrowsNotFoundAsync(string token, string email);
    Task AddAsync(PasswordRecoveryToken? passwordReset);
    Task DeleteAsync(PasswordRecoveryToken? passwordReset);
    Task SaveAsync();
}

public class PasswordResetRepository : IPasswordResetRepository
{
    private readonly AppDbContext _context;

    public PasswordResetRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<PasswordRecoveryToken?> GetByTokenAndEmailThrowsNotFoundAsync(string token, string email)
    {
        return await _context.PasswordRecoveryTokens.FirstOrDefaultAsync(prt => prt.ResetToken == token && prt.User.UserEmail == email);
    }

    public async Task AddAsync(PasswordRecoveryToken? passwordReset)
    {
        await _context.PasswordRecoveryTokens.AddAsync(passwordReset);
    }
    
    public async Task DeleteAsync(PasswordRecoveryToken? passwordReset)
    {
        _context.PasswordRecoveryTokens.Remove(passwordReset);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}