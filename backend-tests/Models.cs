using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID_TESTS;

public static class Models
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
        
        public static readonly User DeactivateHighestPriorityUser = new User
        {
            UserId = Guid.Parse("2c0986d4-040c-4c00-b8f9-31f7a1e71638"), UserUserName = "deactivate-highest-priority",
            UserFirstName = "Highest", UserLastName = "Priority", UserEmail = "highest-priority@test.com",
            UserPassword = HasherHelper.HashString("123456"), IsActive = false
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
            UserRoleId = Guid.Parse("fe904dcf-eeb1-4a71-a229-71185cc15453"), UserId = Users.TestUser.UserId,
            RoleId = Roles.User.RoleId
        };
        
        public static readonly UserRole HighestPriorityUserRoleHighestPriority = new UserRole
        {
            UserRoleId = Guid.Parse("fe904dcf-eeb1-4a71-a229-71185cc15017"), UserId = Users.HighestPriorityUser.UserId,
            RoleId = Roles.HighestPriority.RoleId
        };
        
        public static readonly UserRole DeactivateHighestPriorityUserRoleHighestPriority = new UserRole
        {
            UserRoleId = Guid.Parse("fe000dcf-eeb1-4a71-a229-71185cc15017"), UserId = Users.DeactivateHighestPriorityUser.UserId,
            RoleId = Roles.HighestPriority.RoleId
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
            PermissionName = "Assign_Role",
            PermissionDescription = "Assign role to user"
        };
        
        public static readonly Permission RevokeRole = new Permission
        {
            PermissionId = Guid.Parse("47a2f03a-5f0b-4d73-b535-200a643e7849"),
            PermissionName = "Revoke_Role",
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
        
        public static readonly Permission ActivateUser = new Permission
        {
            PermissionId = Guid.Parse("a43b1178-931e-4eed-9742-30af024ec05b"),
            PermissionName = "Activate_User",
            PermissionDescription = "Activate user"
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
        
        public static readonly Resource ProductsResource = new Resource
        {
            ResourceId = Guid.Parse("a1a2c3d4-1a2c-4a7e-95e0-00bb27dbf7f9"),
            ResourceName = "Products",
            ResourceDescription = "Products module"
        };
        
        public static readonly Resource BrandsResource = new Resource
        {
            ResourceId = Guid.Parse("6691c50e-5bad-4cc3-b035-e2d84ea90c7f"),
            ResourceName = "Brands",
            ResourceDescription = "Brands module"
        };
        
        public static readonly Resource InventoryResource = new Resource
        {
            ResourceId = Guid.Parse("b0f8c2e0-f5a1-4a3e-b5e5-c4e8f0f9c7b7"),
            ResourceName = "Inventory",
            ResourceDescription = "Inventory module"
        };
    }

    public static class RolesPermissionsResources
    {
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
        
        public static readonly RolePermissionResource AdminActivateUser = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.ActivateUser.PermissionId,
            ResourceId = null
        };

        public static readonly RolePermissionResource AdminEditContentUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.EditContent.PermissionId,
            ResourceId = Resources.Users.ResourceId
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
        
        public static readonly RolePermissionResource AdminViewProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.ProductsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.BrandsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminCreateBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.Create.PermissionId,
            ResourceId = Resources.BrandsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminEditBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.EditContent.PermissionId,
            ResourceId = Resources.BrandsResource.ResourceId
        };

        public static readonly RolePermissionResource AdminCreateProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.Create.PermissionId,
            ResourceId = Resources.ProductsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminEditProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.EditContent.PermissionId,
            ResourceId = Resources.ProductsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminCreateInventory = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.Create.PermissionId,
            ResourceId = Resources.InventoryResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminEditInventory = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.EditContent.PermissionId,
            ResourceId = Resources.InventoryResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewInventory = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(),
            RoleId = Roles.Admin.RoleId,
            PermissionId = Permissions.View.PermissionId,
            ResourceId = Resources.InventoryResource.ResourceId
        };
    }

    public static class RefreshTokens
    {
        public static readonly RefreshToken TestUserRefreshTokenRevoked = new RefreshToken
        {
            UserId = Models.Users.TestUser.UserId,
            Token = "zVrwFaCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs=",
            DeviceId = "1",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            RevokedAt = DateTime.UtcNow
        };
        
        public static readonly RefreshToken TestUserExpiredRefreshToken = new RefreshToken
        {
            UserId = Models.Users.TestUser.UserId,
            Token = "zVrwFcCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs=",
            DeviceId = "1",
            ExpiresAt = DateTime.UtcNow.AddHours(-1),
            RevokedAt = null
        };
    }

    public static class PasswordRecoveryTokens
    {
        public static PasswordRecoveryToken TestValidTokenAsync = new PasswordRecoveryToken
        {
            ResetId = Guid.NewGuid(),
            UserId = Models.Users.TestUser.UserId,
            ResetToken = "valid-reset-token",
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };

        public static PasswordRecoveryToken TestExpiredTokenAsync = new PasswordRecoveryToken
        {
            ResetId = Guid.NewGuid(),
            UserId = Models.Users.TestUser.UserId,
            ResetToken = "expired-reset-token",
            ExpiresAt = DateTime.UtcNow.AddHours(-1)
        };
    }
    
    public static class Brands
    {
        public static readonly Brand Apple = new Brand
        {
            BrandId = Guid.Parse("c3146b6f-b50f-4b26-8e77-827fc538b7d1"),
            BrandName = "Apple",
            BrandDescription = "Apple Inc. - Premium electronics",
            IsActive = true
        };
        
        public static readonly Brand Samsung = new Brand
        {
            BrandId = Guid.Parse("809fb57d-ff80-496f-88c3-7b50f0d9b55d"),
            BrandName = "Samsung",
            BrandDescription = "Samsung Electronics - Leading technology company",
            IsActive = true
        };
        
        public static readonly Brand Nike = new Brand
        {
            BrandId = Guid.Parse("5b23b9a8-bd17-4b2a-8e61-b9863a8f77b5"),
            BrandName = "Nike",
            BrandDescription = "Nike Inc. - Sportswear and equipment",
            IsActive = true
        };
    }
    
    public static class Products
    {
        public static readonly Product iPhone13 = new Product
        {
            ProductId = Guid.Parse("5b22cc12-191b-4fe9-9878-bbc1575fa8a7"),
            ProductName = "iPhone 13",
            ProductPrice = 999.99m,
            ProductDescription = "Latest iPhone model",
            IsActive = true,
            BrandId = Brands.Apple.BrandId
        };
        
        public static readonly Product MacBookPro = new Product
        {
            ProductId = Guid.Parse("420a7d01-dd70-417f-872f-aa9f1e1df436"),
            ProductName = "MacBook Pro",
            ProductPrice = 1999.99m,
            ProductDescription = "High-performance laptop",
            IsActive = true,
            BrandId = Brands.Apple.BrandId
        };
        
        public static readonly Product iPadPro = new Product
        {
            ProductId = Guid.Parse("f96bf5e4-9579-404e-9350-087b1ef1305e"),
            ProductName = "iPad Pro",
            ProductPrice = 799.99m,
            ProductDescription = "Powerful tablet for work and entertainment",
            IsActive = true,
            BrandId = Brands.Apple.BrandId
        };
        
        public static readonly Product GalaxyS21 = new Product
        {
            ProductId = Guid.Parse("3179499a-621c-4c07-8acb-e7a057cf4753"),
            ProductName = "Galaxy S21",
            ProductPrice = 899.99m,
            ProductDescription = "Samsung flagship phone",
            IsActive = true,
            BrandId = Brands.Samsung.BrandId
        };
        
        public static readonly Product GalaxyTabS7 = new Product
        {
            ProductId = Guid.Parse("b342289c-c165-4b85-ab6d-2b030f68b171"),
            ProductName = "Galaxy Tab S7",
            ProductPrice = 649.99m,
            ProductDescription = "High-end Android tablet",
            IsActive = true,
            BrandId = Brands.Samsung.BrandId
        };
        
        public static readonly Product SamsungQLEDTV = new Product
        {
            ProductId = Guid.Parse("aa93da89-ba99-4b47-823c-26c5332f6600"),
            ProductName = "Samsung QLED TV",
            ProductPrice = 1500.00m,
            ProductDescription = "Smart TV with stunning display",
            IsActive = true,
            BrandId = Brands.Samsung.BrandId
        };
        
        public static readonly Product NikeAirMax270 = new Product
        {
            ProductId = Guid.Parse("de30da34-c216-488e-8b3b-3cba0fb71baa"),
            ProductName = "Nike Air Max 270",
            ProductPrice = 120.00m,
            ProductDescription = "Comfortable running shoes",
            IsActive = true,
            BrandId = Brands.Nike.BrandId
        };
        
        public static readonly Product NikeZoomX = new Product
        {
            ProductId = Guid.Parse("e9ade468-590a-4c0e-bfbe-91ab9dbb6830"),
            ProductName = "Nike ZoomX Vaporfly Next%",
            ProductPrice = 250.00m,
            ProductDescription = "High-performance running shoes",
            IsActive = true,
            BrandId = Brands.Nike.BrandId
        };
        
        public static readonly Product NikeDriFitTShirt = new Product
        {
            ProductId = Guid.Parse("ac99dcd4-b451-416d-bb12-59b706c5db30"),
            ProductName = "Nike Dri-FIT T-shirt",
            ProductPrice = 30.00m,
            ProductDescription = "Breathable athletic shirt",
            IsActive = true,
            BrandId = Brands.Nike.BrandId
        };
    }

    public static class InventoryModels
    {
        public static readonly Inventory iPhone13Inventory = new Inventory
        {
            InventoryId = Guid.Parse("a1f2e9ca-c431-4e79-aa8a-bb1cbbc5e052"),
            ProductId = Products.iPhone13.ProductId,
            Quantity = 10,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        public static readonly Inventory MacBookProInventory = new Inventory
        {
            InventoryId = Guid.Parse("6a0e1a69-bdc2-435e-8593-3f1fb792fae3"),
            ProductId = Products.MacBookPro.ProductId,
            Quantity = 20,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        public static readonly Inventory iPadProInventory = new Inventory
        {
            InventoryId = Guid.Parse("822c5560-71a2-4641-902f-b35d7b7c77a8"),
            ProductId = Products.iPadPro.ProductId,
            Quantity = 30,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        public static readonly Inventory GalaxyS21Inventory = new Inventory
        {
            InventoryId = Guid.Parse("39ecd16c-be8d-4699-afc1-e6283fd668fc"),
            ProductId = Products.GalaxyS21.ProductId,
            Quantity = 40,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        public static readonly Inventory GalaxyTabS7Inventory = new Inventory
        {
            InventoryId = Guid.Parse("8467ee64-c785-4106-8073-22ddbd891c9c"),
            ProductId = Products.GalaxyTabS7.ProductId,
            Quantity = 50,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        public static readonly Inventory SamsungQLEDTVInventory = new Inventory
        {
            InventoryId = Guid.Parse("922e4874-a46f-452f-8f61-46d87ddd8b06"),
            ProductId = Products.SamsungQLEDTV.ProductId,
            Quantity = 60,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        public static readonly Inventory NikeAirMax270Inventory = new Inventory
        {
            InventoryId = Guid.Parse("71b94117-8ba4-40bb-bd6f-53965ad9edc2"),
            ProductId = Products.NikeAirMax270.ProductId,
            Quantity = 70,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        public static readonly Inventory NikeZoomXInventory = new Inventory
        {
            InventoryId = Guid.Parse("b11b2d59-00bc-49c0-90e4-fc599a04a0d0"),
            ProductId = Products.NikeZoomX.ProductId,
            Quantity = 80,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };
    }
}