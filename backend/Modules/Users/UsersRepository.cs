using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRepository
{
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

    public void Add(User user)
    {
        this._context.Users.Add(user);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}