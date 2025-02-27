using CRM_ERP_UNID.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermissionResource> RolesPermissionsResources { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<UserRole> UsersRoles { get; set; }
}