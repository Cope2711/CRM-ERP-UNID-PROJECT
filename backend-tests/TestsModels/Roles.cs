using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Roles
    {
        public static readonly Role Admin = new Role
        {
            RoleId = Guid.Parse("aad0f879-79bf-42b5-b829-3e14b9ef0e4b"),
            RoleName = "Admin",
            RolePriority = 10f,
            RoleDescription = "Admin role"
        };

        public static readonly Role User = new Role
        {
            RoleId = Guid.Parse("523a8c97-735e-41f7-b4b2-16f92791adf5"),
            RoleName = "User",
            RolePriority = 5f,
            RoleDescription = "User role"
        };

        public static readonly Role Guest = new Role
        {
            RoleId = Guid.Parse("d9b540dd-7e8e-4aa8-a97c-3cdf3a4b08d4"),
            RoleName = "Guest",
            RolePriority = 4.5f,
            RoleDescription = "Guest role"
        };
        
        public static readonly Role HighestPriority = new Role
        {
            RoleId = Guid.Parse("d9b540dd-7e8e-4aa8-a97c-3cdf3a4b28d0"),
            RoleName = "Highest Priority",
            RolePriority = 100f,
            RoleDescription = "Role with highest priority for tests"
        };
    }
}