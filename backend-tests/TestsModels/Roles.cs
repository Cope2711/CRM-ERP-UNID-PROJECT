using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class Roles
    {
        public static readonly Role Admin = new Role
        {
            id = Guid.Parse("aad0f879-79bf-42b5-b829-3e14b9ef0e4b"),
            name = "Admin",
            priority = 10f,
            description = "Admin role"
        };

        public static readonly Role User = new Role
        {
            id = Guid.Parse("523a8c97-735e-41f7-b4b2-16f92791adf5"),
            name = "User",
            priority = 5f,
            description = "User role"
        };

        public static readonly Role Guest = new Role
        {
            id = Guid.Parse("d9b540dd-7e8e-4aa8-a97c-3cdf3a4b08d4"),
            name = "Guest",
            priority = 4.5f,
            description = "Guest role"
        };
        
        public static readonly Role HighestPriority = new Role
        {
            id = Guid.Parse("d9b540dd-7e8e-4aa8-a97c-3cdf3a4b28d0"),
            name = "Highest Priority",
            priority = 100f,
            description = "Role with highest priority for tests"
        };
    }
}