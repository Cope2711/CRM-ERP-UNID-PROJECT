using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Users
    {
        public static readonly User Admin = new User
        {
            id = Guid.Parse("172422a0-5164-4470-acae-72022d3820b1"), userName = "admin",
            firstName = "Admin", lastName = "User", email = "admin@admin.com",
            password = HasherHelper.HashString("123456"), isActive = true
        };

        public static readonly User InactiveTestUser = new User
        {
            id = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e72259"), userName = "test-user",
            firstName = "Test", lastName = "User", email = "test-user@test.com",
            password = HasherHelper.HashString("123456"), isActive = false
        };
        
        public static readonly User TestUser = new User
        {
            id = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e72258"), userName = "test-user2",
            firstName = "Test2", lastName = "User2", email = "test-user2@test.com",
            password = HasherHelper.HashString("123456"), isActive = true
        };
        
        public static readonly User HighestPriorityUser = new User
        {
            id = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e71638"), userName = "highest-priority",
            firstName = "Highest", lastName = "Priority", email = "highest-priority@test.com",
            password = HasherHelper.HashString("123456"), isActive = true
        };
        
        public static readonly User HighestPriorityUser2 = new User
        {
            id = Guid.Parse("d1dbd11d-0644-41b5-bb15-641cca9611a9"), userName = "highest-priority2",
            firstName = "Highest", lastName = "Priority2", email = "highest-priority2@test.com",
            password = HasherHelper.HashString("123456"), isActive = true
        };
        
        public static readonly User DeactivateHighestPriorityUser = new User
        {
            id = Guid.Parse("2c0986d4-040c-4c00-b8f9-31f7a1e71638"), userName = "deactivate-highest-priority",
            firstName = "Highest", lastName = "Priority", email = "highest-priority@test.com",
            password = HasherHelper.HashString("123456"), isActive = false
        };
        
        public static readonly User TestUser2 = new User
        {
            id = Guid.Parse("8f319401-1f2c-4830-9c9b-9efff3aa56a7"), userName = "test-user2",
            firstName = "Test2", lastName = "User2", email = "test-user2@test.com",
            password = HasherHelper.HashString("123456"), isActive = true
        };
    }
}