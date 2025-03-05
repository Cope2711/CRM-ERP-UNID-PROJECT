using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IPasswordResetRepository
{
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