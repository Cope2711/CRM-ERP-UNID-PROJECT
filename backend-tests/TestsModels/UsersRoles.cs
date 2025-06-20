using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class UsersRoles
    {
        public static readonly UserRole AdminUserRoleAdmin = new UserRole
        {
            id = Guid.Parse("842193b4-5048-4cd9-be60-b7ca34319286"), userId = Users.Admin.id,
            roleId = Roles.Admin.id
        };

        public static readonly UserRole TestUserRoleUser = new UserRole
        {
            id = Guid.Parse("fe904dcf-eeb1-4a71-a229-71185cc15453"), userId = Users.TestUser.id,
            roleId = Roles.User.id
        };

        public static readonly UserRole TestUser2RoleUser = new UserRole
        {
            id = Guid.Parse("4dc5a565-c484-48a5-bc15-db1ae421dba0"), userId = Users.TestUser2.id,
            roleId = Roles.User.id
        };

        public static readonly UserRole HighestPriorityUserRoleHighestPriority = new UserRole
        {
            id = Guid.Parse("fe904dcf-eeb1-4a71-a229-71185cc15017"), userId = Users.HighestPriorityUser.id,
            roleId = Roles.HighestPriority.id
        };

        public static readonly UserRole HighestPriorityUserRoleHighestPriority2 = new UserRole
        {
            id = Guid.Parse("36d3b348-09e4-47e2-9205-bb1cd32b9df3"), userId = Users.HighestPriorityUser2.id,
            roleId = Roles.HighestPriority.id
        };

        public static readonly UserRole DeactivateHighestPriorityUserRoleHighestPriority = new UserRole
        {
            id = Guid.Parse("fe000dcf-eeb1-4a71-a229-71185cc15017"),
            userId = Users.DeactivateHighestPriorityUser.id,
            roleId = Roles.HighestPriority.id
        };
    }
}