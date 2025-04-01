using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class UsersRoles
    {
        public static readonly UserRole AdminUserRoleAdmin = new UserRole
        {
            UserRoleId = Guid.Parse("842193b4-5048-4cd9-be60-b7ca34319286"), UserId = Users.Admin.UserId,
            RoleId = Roles.Admin.RoleId
        };

        public static readonly UserRole TestUserRoleUser = new UserRole
        {
            UserRoleId = Guid.Parse("fe904dcf-eeb1-4a71-a229-71185cc15453"), UserId = Users.TestUser.UserId,
            RoleId = Roles.User.RoleId
        };

        public static readonly UserRole TestUser2RoleUser = new UserRole
        {
            UserRoleId = Guid.Parse("4dc5a565-c484-48a5-bc15-db1ae421dba0"), UserId = Users.TestUser2.UserId,
            RoleId = Roles.User.RoleId
        };

        public static readonly UserRole HighestPriorityUserRoleHighestPriority = new UserRole
        {
            UserRoleId = Guid.Parse("fe904dcf-eeb1-4a71-a229-71185cc15017"), UserId = Users.HighestPriorityUser.UserId,
            RoleId = Roles.HighestPriority.RoleId
        };

        public static readonly UserRole HighestPriorityUserRoleHighestPriority2 = new UserRole
        {
            UserRoleId = Guid.Parse("36d3b348-09e4-47e2-9205-bb1cd32b9df3"), UserId = Users.HighestPriorityUser2.UserId,
            RoleId = Roles.HighestPriority.RoleId
        };

        public static readonly UserRole DeactivateHighestPriorityUserRoleHighestPriority = new UserRole
        {
            UserRoleId = Guid.Parse("fe000dcf-eeb1-4a71-a229-71185cc15017"),
            UserId = Users.DeactivateHighestPriorityUser.UserId,
            RoleId = Roles.HighestPriority.RoleId
        };
    }
}