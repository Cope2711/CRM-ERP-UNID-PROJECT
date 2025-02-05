using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace Tests;

public class DatabaseSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Roles.AddRange(
            new Role
            {
                RoleId = Guid.Parse("735250a8-d410-4f77-870a-4422ab28a1a1"),
                RoleName = "Admin"
            },
            new Role
            {
                RoleId = Guid.Parse("668d5a3f-a157-475a-8f77-4212beaf8362"),
                RoleName = "User"
            },
            new Role
            {
                RoleId = Guid.Parse("d6bb84ac-6ca5-4e2b-a977-bc985364315c"),
                RoleName = "Guest"
            }
        );

        context.Users.AddRange(
            new User
            {
                UserId = Guid.Parse("d7f9ed1e-417e-46c0-98f3-df8d63e1e8b6"),
                UserUserName = "admin",
                UserFirstName = "Admin",
                UserLastName = "User",
                UserEmail = "admin@admin.com",
                UserPassword = "$2a$10$H/STMY/cHyRA4LHxLJMUWuajKp4Fw5TiKF.UdGo5hzKqQWTMshKlW",
                IsActive = true,
                RoleId = Guid.Parse("735250a8-d410-4f77-870a-4422ab28a1a1")
            },
            new User
            {
                UserId = Guid.Parse("52184874-eff7-4073-bc7c-9034ccb7d90c"),
                UserUserName = "test-user",
                UserFirstName = "Test",
                UserLastName = "User",
                UserEmail = "test-user@test.com",
                UserPassword = "$2b$12$H4hFo5E9XkP5vwsWfvBi8ea.uh1Vz/5RrG0k3Wu3CC5Y1DuhLK3We",
                IsActive = true,
                RoleId = Guid.Parse("668d5a3f-a157-475a-8f77-4212beaf8362")
            },
            new User
            {
                UserId = Guid.Parse("d7f9ed1e-417e-46c0-98f3-df8d63e1e8b7"),
                UserUserName = "janedoe",
                UserFirstName = "Jane",
                UserLastName = "Doe",
                UserEmail = "janedoe@example.com",
                UserPassword = "$2b$12$E1mQSHZOn1zAYZ0oBua0d1ZZk3jrbxd5OxK0tYd2KZ67xJlmTT4dy",
                IsActive = false,
                RoleId = Guid.Parse("d6bb84ac-6ca5-4e2b-a977-bc985364315c")
            }
        );

        context.RefreshTokens.AddRange(
            new RefreshToken
            {
                RefreshTokenId = Guid.Parse("a1982c18-e3f3-4cd4-4fd0-08dd3d38c53f"),
                UserId = Guid.Parse("d7f9ed1e-417e-46c0-98f3-df8d63e1e8b6"),
                Token = "+9wpQEQ3YJsBXCzLbutUMyIwGo1RenCAh7iKSCQEugg-",
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                RevokedAt = null
            },
            new RefreshToken
            {
                RefreshTokenId = Guid.Parse("a1982c18-e3f3-4cd4-4fd0-08dd3d38c53e"),
                UserId = Guid.Parse("d7f9ed1e-417e-46c0-98f3-df8d63e1e8b6"),
                Token = "+9wpQEQ3YJsBXCzLbutUMyIwGo1RenCAh7iKSCQEugg=",
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                RevokedAt = DateTime.UtcNow
            },
            new RefreshToken
            {
                RefreshTokenId = Guid.Parse("1133349f-8f30-4d75-ff2a-08dd3da33264"),
                UserId = Guid.Parse("d7f9ed1e-417e-46c0-98f3-df8d63e1e8b6"),
                Token = "zVrwFaCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs=",
                ExpiresAt = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                RevokedAt = null
            }
        );


        context.SaveChanges();
    }
}