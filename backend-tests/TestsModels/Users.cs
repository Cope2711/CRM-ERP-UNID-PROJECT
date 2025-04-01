using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Users
    {
        public static readonly User Admin = new User
        {
            UserId = Guid.Parse("172422a0-5164-4470-acae-72022d3820b1"), UserUserName = "admin",
            UserFirstName = "Admin", UserLastName = "User", UserEmail = "admin@admin.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = true
        };

        public static readonly User InactiveTestUser = new User
        {
            UserId = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e72259"), UserUserName = "test-user",
            UserFirstName = "Test", UserLastName = "User", UserEmail = "test-user@test.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = false
        };
        
        public static readonly User TestUser = new User
        {
            UserId = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e72258"), UserUserName = "test-user2",
            UserFirstName = "Test2", UserLastName = "User2", UserEmail = "test-user2@test.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = true
        };
        
        public static readonly User HighestPriorityUser = new User
        {
            UserId = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e71638"), UserUserName = "highest-priority",
            UserFirstName = "Highest", UserLastName = "Priority", UserEmail = "highest-priority@test.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = true
        };
        
        public static readonly User HighestPriorityUser2 = new User
        {
            UserId = Guid.Parse("d1dbd11d-0644-41b5-bb15-641cca9611a9"), UserUserName = "highest-priority2",
            UserFirstName = "Highest", UserLastName = "Priority2", UserEmail = "highest-priority2@test.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = true
        };
        
        public static readonly User DeactivateHighestPriorityUser = new User
        {
            UserId = Guid.Parse("2c0986d4-040c-4c00-b8f9-31f7a1e71638"), UserUserName = "deactivate-highest-priority",
            UserFirstName = "Highest", UserLastName = "Priority", UserEmail = "highest-priority@test.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = false
        };
        
        public static readonly User TestUser2 = new User
        {
            UserId = Guid.Parse("8f319401-1f2c-4830-9c9b-9efff3aa56a7"), UserUserName = "test-user2",
            UserFirstName = "Test2", UserLastName = "User2", UserEmail = "test-user2@test.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = true
        };
    }
}