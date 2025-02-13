using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS;

public static class Models
{
    public static class Roles
    {
        public static readonly Role Admin = new Role
        {
            RoleId = Guid.Parse("aad0f879-79bf-42b5-b829-3e14b9ef0e4b"),
            RoleName = "Admin",
            RoleDescription = "Admin role"
        };

        public static readonly Role User = new Role
        {
            RoleId = Guid.Parse("523a8c97-735e-41f7-b4b2-16f92791adf5"),
            RoleName = "User",
            RoleDescription = "User role"
        };

        public static readonly Role Guest = new Role
        {
            RoleId = Guid.Parse("d9b540dd-7e8e-4aa8-a97c-3cdf3a4b08d4"),
            RoleName = "Guest",
            RoleDescription = "Guest role"
        };
    }

    public static class Users
    {
        public static readonly User Admin = new User
        {
            UserId = Guid.Parse("172422a0-5164-4470-acae-72022d3820b1"), UserUserName = "admin",
            UserFirstName = "Admin", UserLastName = "User", UserEmail = "admin@admin.com",
            UserPassword = "$2a$10$H/STMY/cHyRA4LHxLJMUWuajKp4Fw5TiKF.UdGo5hzKqQWTMshKlW", IsActive = true
        };

        public static readonly User InactiveTestUser = new User
        {
            UserId = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e72259"), UserUserName = "test-user",
            UserFirstName = "Test", UserLastName = "User", UserEmail = "test-user@test.com",
            UserPassword = "$2b$12$H4hFo5E9XkP5vwsWfvBi8ea.uh1Vz/5RrG0k3Wu3CC5Y1DuhLK3We", IsActive = false
        };
        
        public static readonly User TestUser = new User
        {
            UserId = Guid.Parse("2c0180d4-040c-4c00-b8f9-31f7a1e72258"), UserUserName = "test-user2",
            UserFirstName = "Test2", UserLastName = "User2", UserEmail = "test-user2@test.com",
            UserPassword = "$2a$10$H/STMY/cHyRA4LHxLJMUWuajKp4Fw5TiKF.UdGo5hzKqQWTMshKlW", IsActive = true
        };
    }

    public static class UsersRoles
    {
        public static readonly UserRole AdminUserRoleAdmin = new UserRole
        {
            UserRoleId = Guid.Parse("842193b4-5048-4cd9-be60-b7ca34319286"), UserId = Users.Admin.UserId,
            RoleId = Roles.Admin.RoleId
        };

        public static readonly UserRole TestUserRoleUser = new UserRole
        {
            UserRoleId = Guid.Parse("fe904dcf-eeb1-4a71-a229-71185cc15453"), UserId = Users.InactiveTestUser.UserId,
            RoleId = Roles.User.RoleId
        };
    }

    public static class Permissions
    {
        public static readonly Permission View = new Permission
        {
            PermissionId = Guid.Parse("7521ffd2-80e6-4970-8ab3-0d454a377d22"),
            PermissionName = "View",
            PermissionDescription = "Ability to manage users"
        };
        
        public static readonly Permission ViewReports = new Permission
        {
            PermissionId = Guid.Parse("a5088356-4272-4939-b18b-971811fd29e8"),
            PermissionName = "View_Reports",
            PermissionDescription = "Access to view reports"
        };
        
        public static readonly Permission EditContent = new Permission
        {
            PermissionId = Guid.Parse("2a831d9d-1245-451e-8b02-de6542f74574"),
            PermissionName = "Edit_Content",
            PermissionDescription = "Permission to edit content"
        };
        
        public static readonly Permission Create = new Permission
        {
            PermissionId = Guid.Parse("99f766ee-3fd5-4e33-9771-d3821322acea"),
            PermissionName = "Create",
            PermissionDescription = "Create objects"
        };
        
        public static readonly Permission AssignRole = new Permission
        {
            PermissionId = Guid.Parse("5c748c35-a4f5-48d6-a320-32287c8649a9"),
            PermissionName = "Assign_Role_To_User",
            PermissionDescription = "Assign role to user"
        };
        
        public static readonly Permission RevokeRole = new Permission
        {
            PermissionId = Guid.Parse("47a2f03a-5f0b-4d73-b535-200a643e7849"),
            PermissionName = "Revoke_Role_To_User",
            PermissionDescription = "Revoke role to user"
        };
        
        public static readonly Permission AssignPermission = new Permission
        {
            PermissionId = Guid.Parse("554b4b5a-cae7-414c-91f8-75df725b526d"),
            PermissionName = "Assign_Permission",
            PermissionDescription = "Assign permission to role"
        };
        
        public static readonly Permission RevokePermission = new Permission
        {
            PermissionId = Guid.Parse("9037e10c-38ea-40a6-b364-d68f86203c11"),
            PermissionName = "Revoke_Permission",
            PermissionDescription = "Revoke permission to role"
        };
        
        public static readonly Permission Delete = new Permission
        {
            PermissionId = Guid.Parse("722399bc-76f4-4bfa-950d-85e8b93f7af5"),
            PermissionName = "Delete",
            PermissionDescription = "Delete objects"
        };
        
        public static readonly Permission DeactivateUser = new Permission
        {
            PermissionId = Guid.Parse("10d321bd-b667-40c9-adb0-50e62d37c4cc"),
            PermissionName = "Deactivate_User",
            PermissionDescription = "Deactivate user"
        };
    }

    public static class Resources
    {
        public static readonly Resource Users = new Resource
        {
            ResourceId = Guid.Parse("d161ec8c-7c31-4eb4-a331-82ef9e45903e"),
            ResourceName = "Users",
            ResourceDescription = "Users module"
        };
        
        public static readonly Resource Roles = new Resource
        {
            ResourceId = Guid.Parse("5688d987-b031-4780-af66-1a99f2fa69dd"),
            ResourceName = "Roles",
            ResourceDescription = "Roles module"
        };
        
        public static readonly Resource Permissions = new Resource
        {
            ResourceId = Guid.Parse("09e63ef5-71ca-4dcb-8f69-4997b02c1e6d"),
            ResourceName = "Permissions",
            ResourceDescription = "Permissions module"
        };
        
        public static readonly Resource UsersRoles = new Resource
        {
            ResourceId = Guid.Parse("85fac418-d875-4f3c-8094-c2d614a58f15"),
            ResourceName = "UsersRoles",
            ResourceDescription = "Users roles module"
        };
        
        public static readonly Resource RolesPermissionsResources = new Resource
        {
            ResourceId = Guid.Parse("67f53f8f-1848-4156-8ee9-ec9e02bd5836"),
            ResourceName = "RolesPermissionsResources",
            ResourceDescription = "Roles permissions resources module"
        };
        
        public static readonly Resource ResourcesResource = new Resource
        {
            ResourceId = Guid.Parse("6193cd07-1a2c-4a7e-95e0-00bb27dbf7c3"),
            ResourceName = "Resources",
            ResourceDescription = "Resources module"
        };
    }

    public static class RolesPermissionsResources
    {
        /*-- Insertar Permisos a los Roles
INSERT INTO RolesPermissionsResources (RolePermissionId, RoleId, PermissionId, ResourceId)
VALUES (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Users),
       (NEWID(), @RoleId_Admin, @PermissionId_DeactivateUser, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Users),
       (NEWID(), @RoleId_Admin, @PermissionId_AssignRole, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_RevokeRole, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_UsersRoles),
       (NEWID(), @RoleId_Admin, @PermissionId_AssignPermission, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_RevokePermission, NULL),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_RolesPermissionsResources),
       (NEWID(), @RoleId_Admin, @PermissionId_EditContent, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_Create, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_Delete, @ResourceId_Roles),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Resources),
       (NEWID(), @RoleId_Admin, @PermissionId_View, @ResourceId_Permissions)*/
        public static readonly RolePermissionResource AdminViewUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.Users.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeactivateUser = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.DeactivateUser.PermissionId,
            ResourceId = null
        };
        
        public static readonly RolePermissionResource AdminEditContent = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.EditContent.PermissionId,
            ResourceId = null
        };
        
        public static readonly RolePermissionResource AdminCreateUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.Create.PermissionId,
            ResourceId = Resources.Users.ResourceId
        };
        
        public static readonly RolePermissionResource AdminAssignRole = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.AssignRole.PermissionId,
            ResourceId = null
        };
        
        public static readonly RolePermissionResource AdminRevokeRole = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.RevokeRole.PermissionId,
            ResourceId = null
        };
        
        public static readonly RolePermissionResource AdminViewUsersRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.UsersRoles.ResourceId
        };
        
        public static readonly RolePermissionResource AdminAssignPermission = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.AssignPermission.PermissionId,
            ResourceId = null
        };
        
        public static readonly RolePermissionResource AdminRevokePermission = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.RevokePermission.PermissionId,
            ResourceId = null
        };
        
        public static readonly RolePermissionResource AdminViewRolesPermissionsResources = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.RolesPermissionsResources.ResourceId
        };
        
        public static readonly RolePermissionResource AdminEditRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.EditContent.PermissionId,
            ResourceId = Resources.Roles.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.Roles.ResourceId
        };
        
        public static readonly RolePermissionResource AdminCreateRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.Create.PermissionId,
            ResourceId = Resources.Roles.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeleteRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.Delete.PermissionId,
            ResourceId = Resources.Roles.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewResources = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.ResourcesResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewPermissions = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.Permissions.ResourceId
        };
    }
}