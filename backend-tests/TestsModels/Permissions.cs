using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Permissions
    {
        public static readonly Permission Assign = new Permission
        {
            PermissionId = Guid.Parse("a5651c2a-4847-4eaa-b85f-d4f2cba2184a"),
            PermissionName = "Assign",
            PermissionDescription = "Assign objects"
        };

        public static readonly Permission Revoke = new Permission
        {
            PermissionId = Guid.Parse("4e11ee1b-1cd8-4f01-9e00-5dbd3df5620c"),
            PermissionName = "Revoke",
            PermissionDescription = "Revoke objects"
        };
        
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
        
        public static readonly Permission ActivateUser = new Permission
        {
            PermissionId = Guid.Parse("a43b1178-931e-4eed-9742-30af024ec05b"),
            PermissionName = "Activate_User",
            PermissionDescription = "Activate user"
        };
    }
}