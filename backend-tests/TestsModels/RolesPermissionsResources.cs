using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class RolesPermissionsResources
    {
        public static readonly RolePermissionResource AdminAssignProductsCategories = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Assign.id, resourceId = Resources.ProductsCategories.id
        };
        
        public static readonly RolePermissionResource AdminRevokeProductsCategories = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Revoke.id, resourceId = Resources.ProductsCategories.id
        };
        
        public static readonly RolePermissionResource AdminViewProductsCategories = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.ProductsCategories.id
        };
        
        public static readonly RolePermissionResource AdminViewCategories = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.Categories.id
        };
        
        public static readonly RolePermissionResource AdminCreateCategories = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.Categories.id
        };
        
        public static readonly RolePermissionResource AdminEditCategories = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.Categories.id
        };
        
        public static readonly RolePermissionResource AdminDeleteCategories = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Delete.id, resourceId = Resources.Categories.id
        };

        public static readonly RolePermissionResource AdminEditContentSuppliersBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.SuppliersBranches.id
        };

        public static readonly RolePermissionResource AdminAssignSupplierBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Assign.id, resourceId = Resources.SuppliersBranches.id
        };

        public static readonly RolePermissionResource AdminRevokeSupplierBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Revoke.id, resourceId = Resources.SuppliersBranches.id
        };

        public static readonly RolePermissionResource AdminViewSuppliersBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.SuppliersBranches.id
        };

        public static readonly RolePermissionResource AdminAssignProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Assign.id, resourceId = Resources.SuppliersProducts.id
        };

        public static readonly RolePermissionResource AdminRevokeProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Revoke.id, resourceId = Resources.SuppliersProducts.id
        };
        
        public static readonly RolePermissionResource AdminEditContentSuppliersProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.SuppliersProducts.id
        };

        public static readonly RolePermissionResource AdminViewSuppliersProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.SuppliersProducts.id
        };

        public static readonly RolePermissionResource AdminViewSuppliers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.Suppliers.id
        };

        public static readonly RolePermissionResource AdminEditContentSuppliers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.Suppliers.id
        };

        public static readonly RolePermissionResource AdminCreateSuppliers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.Suppliers.id
        };
        
        public static readonly RolePermissionResource AdminActivateSuppliers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Activate.id, resourceId = Resources.Suppliers.id
        };
        
        public static readonly RolePermissionResource AdminDeactivateSuppliers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Deactivate.id, resourceId = Resources.Suppliers.id
        };

        public static readonly RolePermissionResource AdminAssignUsersBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Assign.id, resourceId = Resources.UsersBranches.id
        };

        public static readonly RolePermissionResource AdminRevokeUsersBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Revoke.id, resourceId = Resources.UsersBranches.id
        };

        public static readonly RolePermissionResource AdminViewUsersBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.UsersBranches.id
        };

        public static readonly RolePermissionResource AdminViewBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.BranchesResource.id
        };

        public static readonly RolePermissionResource AdminCreateBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.BranchesResource.id
        };

        public static readonly RolePermissionResource AdminEditBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.BranchesResource.id
        };
        
        public static readonly RolePermissionResource AdminActivateBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Activate.id, resourceId = Resources.BranchesResource.id
        };
        
        public static readonly RolePermissionResource AdminDeactivateBranches = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Deactivate.id, resourceId = Resources.BranchesResource.id
        };
        
        public static readonly RolePermissionResource AdminActivateUsers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Activate.id, resourceId = Resources.Users.id
        };
        
        public static readonly RolePermissionResource AdminDeactivateUsers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Deactivate.id, resourceId = Resources.Users.id
        };
        
        public static readonly RolePermissionResource AdminViewUsers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.Users.id
        };

        public static readonly RolePermissionResource AdminEditContentUsers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.Users.id
        };
        
        public static readonly RolePermissionResource AdminCreateUsers = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.Users.id
        };

        public static readonly RolePermissionResource AdminAssignUsersRoles = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Assign.id, resourceId = Resources.UsersRoles.id
        };

        public static readonly RolePermissionResource AdminRevokeUsersRoles = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Revoke.id, resourceId = Resources.UsersRoles.id
        };

        public static readonly RolePermissionResource AdminViewUsersRoles = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.UsersRoles.id
        };

        public static readonly RolePermissionResource AdminAssignPermission = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Assign.id, resourceId = Resources.RolesPermissionsResources.id
        };

        public static readonly RolePermissionResource AdminRevokePermission = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Revoke.id, resourceId = Resources.RolesPermissionsResources.id
        };

        public static readonly RolePermissionResource AdminViewRolesPermissionsResources = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.RolesPermissionsResources.id
        };

        public static readonly RolePermissionResource AdminEditRoles = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.Roles.id
        };

        public static readonly RolePermissionResource AdminViewRoles = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.Roles.id
        };

        public static readonly RolePermissionResource AdminCreateRoles = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.Roles.id
        };

        public static readonly RolePermissionResource AdminDeleteRoles = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Delete.id, resourceId = Resources.Roles.id
        };

        public static readonly RolePermissionResource AdminViewResources = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.ResourcesResource.id
        };

        public static readonly RolePermissionResource AdminViewPermissions = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.Permissions.id
        };
        
        public static readonly RolePermissionResource AdminViewBrands = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.BrandsResource.id
        };

        public static readonly RolePermissionResource AdminActivateBrands = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Activate.id, resourceId = Resources.BrandsResource.id
        };
        
        public static readonly RolePermissionResource AdminDeactivateBrands = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Deactivate.id, resourceId = Resources.BrandsResource.id
        };
        
        public static readonly RolePermissionResource AdminCreateBrands = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.BrandsResource.id
        };

        public static readonly RolePermissionResource AdminEditBrands = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.BrandsResource.id
        };

        public static readonly RolePermissionResource AdminCreateProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.ProductsResource.id
        };
        
        public static readonly RolePermissionResource AdminViewProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.ProductsResource.id
        };
        
        public static readonly RolePermissionResource AdminActivateProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Activate.id, resourceId = Resources.ProductsResource.id
        };
        
        public static readonly RolePermissionResource AdminDeactivateProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Deactivate.id, resourceId = Resources.ProductsResource.id
        };

        public static readonly RolePermissionResource AdminEditProducts = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.ProductsResource.id
        };

        public static readonly RolePermissionResource AdminCreateInventory = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.Create.id, resourceId = Resources.InventoryResource.id
        };

        public static readonly RolePermissionResource AdminEditInventory = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.EditContent.id, resourceId = Resources.InventoryResource.id
        };

        public static readonly RolePermissionResource AdminViewInventory = new RolePermissionResource
        {
            id = Guid.NewGuid(), roleId = Roles.Admin.id, permissionId = Permissions.View.id, resourceId = Resources.InventoryResource.id
        };
    }
}