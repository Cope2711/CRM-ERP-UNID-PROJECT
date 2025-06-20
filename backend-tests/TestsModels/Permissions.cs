using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Permissions
    {
        public static readonly Permission Assign = new Permission
        {
            id = Guid.Parse("a5651c2a-4847-4eaa-b85f-d4f2cba2184a"),
            name = "Assign",
            description = "Assign objects"
        };

        public static readonly Permission Revoke = new Permission
        {
            id = Guid.Parse("4e11ee1b-1cd8-4f01-9e00-5dbd3df5620c"),
            name = "Revoke",
            description = "Revoke objects"
        };
        
        public static readonly Permission View = new Permission
        {
            id = Guid.Parse("7521ffd2-80e6-4970-8ab3-0d454a377d22"),
            name = "View",
            description = "Ability to manage users"
        };
        
        public static readonly Permission ViewReports = new Permission
        {
            id = Guid.Parse("a5088356-4272-4939-b18b-971811fd29e8"),
            name = "View_Reports",
            description = "Access to view reports"
        };
        
        public static readonly Permission EditContent = new Permission
        {
            id = Guid.Parse("2a831d9d-1245-451e-8b02-de6542f74574"),
            name = "Edit_Content",
            description = "Permission to edit content"
        };
        
        public static readonly Permission Create = new Permission
        {
            id = Guid.Parse("99f766ee-3fd5-4e33-9771-d3821322acea"),
            name = "Create",
            description = "Create objects"
        };
        
        public static readonly Permission Delete = new Permission
        {
            id = Guid.Parse("722399bc-76f4-4bfa-950d-85e8b93f7af5"),
            name = "Delete",
            description = "Delete objects"
        };
        
        public static readonly Permission Activate = new Permission
        {
            id = Guid.Parse("cafac91d-73d5-4f38-85a8-215d1b82e453"),
            name = "Activate",
            description = "Activate objects"
        };
        
        public static readonly Permission Deactivate = new Permission
        {
            id = Guid.Parse("6707e72f-0120-423f-ace1-e016de411531"),
            name = "Deactivate",
            description = "Deactivate objects"
        };
    }
}