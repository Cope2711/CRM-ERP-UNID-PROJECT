using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules.RecoverPassword;

public interface IPasswordResetRepository
{
    Task AddAsync(PasswordReset? passwordReset);
    Task<PasswordReset?> GetByTokenAsync(string token);
    Task DeleteAsync(PasswordReset? passwordReset);
    Task SaveAsync();
}

public class PasswordResetRepository : IPasswordResetRepository
{
    private readonly AppDbContext _context;

    public PasswordResetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PasswordReset? passwordReset)
    {
        await _context.PasswordResets.AddAsync(passwordReset);
    }

    public async Task<PasswordReset?> GetByTokenAsync(string token)
    {
        return await _context.PasswordResets
            .FirstOrDefaultAsync(pr =>pr.ResetToken == token);
    }

    public async Task DeleteAsync(PasswordReset? passwordReset)
    {
        _context.PasswordResets.Remove(passwordReset);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}