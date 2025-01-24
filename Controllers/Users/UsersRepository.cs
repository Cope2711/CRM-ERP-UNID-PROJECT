using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers;

public interface IUsersRepository
{
    Task<User?> GetById(Guid id);
    Task<User?> GetByUserName(string userName);
    Task<User?> GetByEmail(string email);
    void Add(User user);
    Task SaveChangesAsync();
}

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context)
    {
        this._context = context;
    }
    
    public async Task<User?> GetById(Guid id)
    {
        return await this._context.Users.FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await this._context.Users.FirstOrDefaultAsync(u => u.UserUserName == userName);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await this._context.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
    }

    public void Add(User user)
    {
        this._context.Users.Add(user);
    }
    
    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}