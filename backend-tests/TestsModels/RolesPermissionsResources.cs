using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class RolesPermissionsResources
    {
        public static readonly RolePermissionResource AdminAssignProductsCategories = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Assign.PermissionId, ResourceId = Resources.ProductsCategories.ResourceId
        };
        
        public static readonly RolePermissionResource AdminRevokeProductsCategories = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Revoke.PermissionId, ResourceId = Resources.ProductsCategories.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewProductsCategories = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.ProductsCategories.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewCategories = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.Categories.ResourceId
        };
        
        public static readonly RolePermissionResource AdminCreateCategories = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.Categories.ResourceId
        };
        
        public static readonly RolePermissionResource AdminEditCategories = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.Categories.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeleteCategories = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Delete.PermissionId, ResourceId = Resources.Categories.ResourceId
        };

        public static readonly RolePermissionResource AdminEditContentSuppliersBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.SuppliersBranches.ResourceId
        };

        public static readonly RolePermissionResource AdminAssignSupplierBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Assign.PermissionId, ResourceId = Resources.SuppliersBranches.ResourceId
        };

        public static readonly RolePermissionResource AdminRevokeSupplierBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Revoke.PermissionId, ResourceId = Resources.SuppliersBranches.ResourceId
        };

        public static readonly RolePermissionResource AdminViewSuppliersBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.SuppliersBranches.ResourceId
        };

        public static readonly RolePermissionResource AdminAssignProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Assign.PermissionId, ResourceId = Resources.SuppliersProducts.ResourceId
        };

        public static readonly RolePermissionResource AdminRevokeProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Revoke.PermissionId, ResourceId = Resources.SuppliersProducts.ResourceId
        };
        
        public static readonly RolePermissionResource AdminEditContentSuppliersProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.SuppliersProducts.ResourceId
        };

        public static readonly RolePermissionResource AdminViewSuppliersProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.SuppliersProducts.ResourceId
        };

        public static readonly RolePermissionResource AdminViewSuppliers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.Suppliers.ResourceId
        };

        public static readonly RolePermissionResource AdminEditContentSuppliers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.Suppliers.ResourceId
        };

        public static readonly RolePermissionResource AdminCreateSuppliers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.Suppliers.ResourceId
        };
        
        public static readonly RolePermissionResource AdminActivateSuppliers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Activate.PermissionId, ResourceId = Resources.Suppliers.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeactivateSuppliers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Deactivate.PermissionId, ResourceId = Resources.Suppliers.ResourceId
        };

        public static readonly RolePermissionResource AdminAssignUsersBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Assign.PermissionId, ResourceId = Resources.UsersBranches.ResourceId
        };

        public static readonly RolePermissionResource AdminRevokeUsersBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Revoke.PermissionId, ResourceId = Resources.UsersBranches.ResourceId
        };

        public static readonly RolePermissionResource AdminViewUsersBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.UsersBranches.ResourceId
        };

        public static readonly RolePermissionResource AdminViewBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.BranchesResource.ResourceId
        };

        public static readonly RolePermissionResource AdminCreateBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.BranchesResource.ResourceId
        };

        public static readonly RolePermissionResource AdminEditBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.BranchesResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminActivateBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Activate.PermissionId, ResourceId = Resources.BranchesResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeactivateBranches = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Deactivate.PermissionId, ResourceId = Resources.BranchesResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminActivateUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Activate.PermissionId, ResourceId = Resources.Users.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeactivateUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Deactivate.PermissionId, ResourceId = Resources.Users.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.Users.ResourceId
        };

        public static readonly RolePermissionResource AdminEditContentUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.Users.ResourceId
        };
        
        public static readonly RolePermissionResource AdminCreateUsers = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.Users.ResourceId
        };

        public static readonly RolePermissionResource AdminAssignUsersRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Assign.PermissionId, ResourceId = Resources.UsersRoles.ResourceId
        };

        public static readonly RolePermissionResource AdminRevokeUsersRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Revoke.PermissionId, ResourceId = Resources.UsersRoles.ResourceId
        };

        public static readonly RolePermissionResource AdminViewUsersRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.UsersRoles.ResourceId
        };

        public static readonly RolePermissionResource AdminAssignPermission = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Assign.PermissionId, ResourceId = Resources.RolesPermissionsResources.ResourceId
        };

        public static readonly RolePermissionResource AdminRevokePermission = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Revoke.PermissionId, ResourceId = Resources.RolesPermissionsResources.ResourceId
        };

        public static readonly RolePermissionResource AdminViewRolesPermissionsResources = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.RolesPermissionsResources.ResourceId
        };

        public static readonly RolePermissionResource AdminEditRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.Roles.ResourceId
        };

        public static readonly RolePermissionResource AdminViewRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.Roles.ResourceId
        };

        public static readonly RolePermissionResource AdminCreateRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.Roles.ResourceId
        };

        public static readonly RolePermissionResource AdminDeleteRoles = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Delete.PermissionId, ResourceId = Resources.Roles.ResourceId
        };

        public static readonly RolePermissionResource AdminViewResources = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.ResourcesResource.ResourceId
        };

        public static readonly RolePermissionResource AdminViewPermissions = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.Permissions.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.BrandsResource.ResourceId
        };

        public static readonly RolePermissionResource AdminActivateBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Activate.PermissionId, ResourceId = Resources.BrandsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeactivateBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Deactivate.PermissionId, ResourceId = Resources.BrandsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminCreateBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.BrandsResource.ResourceId
        };

        public static readonly RolePermissionResource AdminEditBrands = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.BrandsResource.ResourceId
        };

        public static readonly RolePermissionResource AdminCreateProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.ProductsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminViewProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.ProductsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminActivateProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Activate.PermissionId, ResourceId = Resources.ProductsResource.ResourceId
        };
        
        public static readonly RolePermissionResource AdminDeactivateProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Deactivate.PermissionId, ResourceId = Resources.ProductsResource.ResourceId
        };

        public static readonly RolePermissionResource AdminEditProducts = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.ProductsResource.ResourceId
        };

        public static readonly RolePermissionResource AdminCreateInventory = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.Create.PermissionId, ResourceId = Resources.InventoryResource.ResourceId
        };

        public static readonly RolePermissionResource AdminEditInventory = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.EditContent.PermissionId, ResourceId = Resources.InventoryResource.ResourceId
        };

        public static readonly RolePermissionResource AdminViewInventory = new RolePermissionResource
        {
            RolePermissionId = Guid.NewGuid(), RoleId = Roles.Admin.RoleId, PermissionId = Permissions.View.PermissionId, ResourceId = Resources.InventoryResource.ResourceId
        };
    }
}