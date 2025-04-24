using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Constants;

public static class Fields
{
    public static class ProductsCategories
    {
        public const string ProductCategoryId = nameof(ProductCategory.ProductCategoryId);
        public const string ProductId = nameof(ProductCategory.ProductId);
        public const string CategoryId = nameof(ProductCategory.CategoryId);
    }
    
    public static class Categories
    {
        public const string CategoryId = nameof(Category.CategoryId);
        public const string CategoryName = nameof(Category.CategoryName);
        public const string CategoryDescription = nameof(Category.CategoryDescription);
    }
    
    public static class SuppliersBranches
    {
        public const string SupplierBranchId = nameof(SupplierBranch.SupplierBranchId);
        public const string SupplierId = nameof(SupplierBranch.SupplierId);
        public const string BranchId = nameof(SupplierBranch.BranchId);
        public const string IsPreferredSupplier = nameof(SupplierBranch.IsPreferredSupplier);
    }
    
    public static class SuppliersProducts
    {
        public const string SupplierProductId = nameof(SupplierProduct.SupplierProductId);
        public const string SupplierId = nameof(SupplierProduct.SupplierId);
        public const string ProductId = nameof(SupplierProduct.ProductId);
        public const string SupplyPrice = nameof(SupplierProduct.SupplyPrice);
        public const string SupplyLeadTime = nameof(SupplierProduct.SupplyLeadTime);
    }
    
    public static class Suppliers
    {
        public const string SupplierId = nameof(Supplier.SupplierId);
        public const string SupplierName = nameof(Supplier.SupplierName);
        public const string SupplierContact = nameof(Supplier.SupplierContact);
        public const string SupplierEmail = nameof(Supplier.SupplierEmail);
        public const string SupplierPhone = nameof(Supplier.SupplierPhone);
        public const string SupplierAddress = nameof(Supplier.SupplierAddress);
        public const string IsActive = nameof(Supplier.IsActive);
    }
    
    public static class Branches
    {
        public const string BranchId = nameof(Branch.BranchId);
        public const string BranchName = nameof(Branch.BranchName);
        public const string BranchAddress = nameof(Branch.BranchAddress);
        public const string BranchPhone = nameof(Branch.BranchPhone);
        public const string IsActive = nameof(Branch.IsActive);
    }
    
    public static class Users
    {
        public const string UserId = nameof(User.UserId);
        public const string UserUserName = nameof(User.UserUserName);
        public const string UserFirstName = nameof(User.UserFirstName);
        public const string UserLastName = nameof(User.UserLastName);
        public const string UserEmail = nameof(User.UserEmail);
        public const string UserPassword = nameof(User.UserPassword);
        public const string IsActive = nameof(User.IsActive);
        public const string UserRoles = nameof(User.UserRoles);
    }
    
    public static class Permissions
    {
        public const string PermissionId = nameof(Permission.PermissionId);
        public const string PermissionName = nameof(Permission.PermissionName);
        public const string PermissionDescription = nameof(Permission.PermissionDescription);
    }
    
    public static class Roles
    {
        public const string RoleId = nameof(Role.RoleId);
        public const string RoleName = nameof(Role.RoleName);
        public const string RoleDescription = nameof(Role.RoleDescription);
        public const string RolePriority = nameof(Role.RolePriority);
    }
    
    public static class Resources
    {
        public const string ResourceId = nameof(Resource.ResourceId);
        public const string ResourceName = nameof(Resource.ResourceName);
        public const string ResourceDescription = nameof(Resource.ResourceDescription);
    }
    
    public static class RefreshTokens
    {
        public const string RefreshTokenField = "RefreshToken";
        public const string DeviceId = nameof(RefreshToken.DeviceId);
    }
    
    public static class UsersRoles
    {
        public const string UserRoleId = nameof(UserRole.UserRoleId);
        public const string UserId = nameof(UserRole.UserId);
        public const string RoleId = nameof(UserRole.RoleId);
    }
    
    public static class PasswordRecoveryTokens
    {
        public const string ResetId = nameof(PasswordRecoveryToken.ResetId);
        public const string UserId = nameof(PasswordRecoveryToken.UserId);
        public const string ResetToken = nameof(PasswordRecoveryToken.ResetToken);
        public const string ExpiresAt = nameof(PasswordRecoveryToken.ExpiresAt);
        public const string CreatedAt = nameof(PasswordRecoveryToken.CreatedAt);
    }
    
    public static class Brands
    {
        public const string BrandId = nameof(Brand.BrandId);
        public const string BrandName = nameof(Brand.BrandName);
        public const string BrandDescription = nameof(Brand.BrandDescription);
        public const string IsActive = nameof(Brand.IsActive);
    }
    
    public static class Products
    {
        public const string ProductId = nameof(Product.ProductId);
        public const string ProductName = nameof(Product.ProductName);
        public const string ProductBarcode = nameof(Product.ProductBarcode);
        public const string ProductPrice = nameof(Product.ProductPrice);
        public const string ProductDescription = nameof(Product.ProductDescription);
        public const string IsActive = nameof(Product.IsActive);
        public const string BrandId = nameof(Product.BrandId);
    }
    
    public static class InventoryFields
    {
        public const string InventoryId = nameof(InventoryFields.InventoryId);
        public const string ProductId = nameof(InventoryFields.ProductId);
        public const string Quantity = nameof(InventoryFields.Quantity);
        public const string IsActive = nameof(InventoryFields.IsActive);
    }
    
    public static class Examples
    {
        public const string ExampleId = nameof(Example.ExampleId);
        public const string ExampleName = nameof(Example.ExampleName);
    }

    public static class Sales
    {
        public const string SaleId = nameof(Sale.SaleId);
    }
}