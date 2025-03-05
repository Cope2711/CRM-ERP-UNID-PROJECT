using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Data;

namespace Tests;

public class DatabaseSeeder
{
    public static void Seed(AppDbContext context)
    {
      
        context.Roles.AddRange(
            Models.Roles.Admin,
            Models.Roles.User,
            Models.Roles.Guest);
        context.Users.AddRange(
            Models.Users.Admin,
            Models.Users.InactiveTestUser,
            Models.Users.TestUser);
        context.UsersRoles.AddRange(
            Models.UsersRoles.AdminUserRoleAdmin,
            Models.UsersRoles.TestUserRoleUser);
        context.Permissions.AddRange(
            Models.Permissions.View,
            Models.Permissions.ViewReports,
            Models.Permissions.EditContent,
            Models.Permissions.Create,
            Models.Permissions.AssignRole,
            Models.Permissions.RevokeRole,
            Models.Permissions.AssignPermission,
            Models.Permissions.RevokePermission,
            Models.Permissions.Delete,
            Models.Permissions.DeactivateUser,
            Models.Permissions.ActivateUser);
        context.Resources.AddRange(
            Models.Resources.Users,
            Models.Resources.Roles,
            Models.Resources.Permissions,
            Models.Resources.UsersRoles,
            Models.Resources.RolesPermissionsResources,
            Models.Resources.ResourcesResource);
        context.RolesPermissionsResources.AddRange(Models.RolesPermissionsResources.AdminViewUsers,
            Models.RolesPermissionsResources.AdminDeactivateUser,
            Models.RolesPermissionsResources.AdminEditContentUsers,
            Models.RolesPermissionsResources.AdminEditContent,
            Models.RolesPermissionsResources.AdminCreateUsers,
            Models.RolesPermissionsResources.AdminAssignRole,
            Models.RolesPermissionsResources.AdminRevokeRole,
            Models.RolesPermissionsResources.AdminViewUsersRoles,
            Models.RolesPermissionsResources.AdminAssignPermission,
            Models.RolesPermissionsResources.AdminRevokePermission,
            Models.RolesPermissionsResources.AdminViewRolesPermissionsResources,
            Models.RolesPermissionsResources.AdminEditRoles,
            Models.RolesPermissionsResources.AdminViewRoles,
            Models.RolesPermissionsResources.AdminCreateRoles,
            Models.RolesPermissionsResources.AdminDeleteRoles,
            Models.RolesPermissionsResources.AdminViewResources,
            Models.RolesPermissionsResources.AdminViewPermissions,
            Models.RolesPermissionsResources.AdminActivateUser);
        context.RefreshTokens.AddRange(
            Models.RefreshTokens.TestUserRefreshTokenRevoked,
            Models.RefreshTokens.TestUserExpiredRefreshToken
        );
        context.PasswordRecoveryTokens.AddRange(
            Models.PasswordRecoveryTokens.TestValidTokenAsynk,
            Models.PasswordRecoveryTokens.TestExpiredTokenAsynk
            );
        context.SaveChanges();
    }
}