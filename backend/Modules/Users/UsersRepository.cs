using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRepository
{
    void Add(User user);
    
    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    Task SaveChangesAsync();
    
    /// <summary>
    /// Persists all changes made in the repository to the underlying data store asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();
    
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
    
    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}