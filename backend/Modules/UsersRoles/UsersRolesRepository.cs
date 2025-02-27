using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesRepository
{
    void Remove(UserRole userRole);
    
    /// <summary>
    /// Removes a user-role association from the repository.
    /// </summary>
    /// <param name="userRole">The user-role association to remove.</param>
    void Add(UserRole userRole);
    
    /// <summary>
    /// Adds a new user-role association to the repository.
    /// </summary>
    /// <param name="userRole">The user-role association to add.</param>
    Task SaveChangesAsync();
    
    /// <summary>
    /// Persists all changes made in the repository to the underlying data store asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId);
    
    /// <summary>
    /// Checks if a specific role is assigned to a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains true if the role is assigned to the user; otherwise, false.
    /// </returns>
    Task<UserRole?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId);
    
    /// <summary>
    /// Retrieves the user-role association for the specified user and role.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the user-role association if found; otherwise, null.
    /// </returns>
}

public class UsersRolesRepository : IUsersRolesRepository
{
    private readonly AppDbContext _context;

    public UsersRolesRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserRole?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId)
    {
        return await this._context.UsersRoles.Include(ur => ur.User).Include(ur => ur.Role).FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
    
    public void Add(UserRole userRole)
    {
        this._context.UsersRoles.Add(userRole);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
    
    public async Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId)
    {
        return await this._context.UsersRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
    
    public void Remove(UserRole userRole)
    {
        _context.UsersRoles.Remove(userRole);
    }
}