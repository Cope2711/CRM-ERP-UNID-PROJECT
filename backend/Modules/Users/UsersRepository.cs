using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRepository
{
    void Add(User user);
    Task SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<User?> GetByEmailAsync(string email);
    
}


public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;
    
    public UsersRepository(AppDbContext context)
    {
        this._context = context;
    }
    public void Add(User user)
    {
        this._context.Users.Add(user);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
       return await this._context.Database.BeginTransactionAsync();
    }
    //recover Password
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
    }
    
    // end recover password
    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }

    
}