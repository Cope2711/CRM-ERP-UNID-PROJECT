using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Constants;

public static class Fields
{
    public static class ProductsCategories
    {
        public const string id = nameof(ProductCategory.id);
        public const string productId = nameof(ProductCategory.productId);
        public const string categoryId = nameof(ProductCategory.categoryId);
    }
    
    public static class Categories
    {
        public const string CategoryId = nameof(Category.id);
        public const string name = nameof(Category.name);
        public const string description = nameof(Category.description);
    }
    
    public static class SuppliersBranches
    {
        public const string id = nameof(SupplierBranch.id);
        public const string supplierId = nameof(SupplierBranch.supplierId);
        public const string branchId = nameof(SupplierBranch.branchId);
        public const string isPreferredSupplier = nameof(SupplierBranch.isPreferredSupplier);
    }
    
    public static class SuppliersProducts
    {
        public const string id = nameof(SupplierProduct.id);
        public const string supplierId = nameof(SupplierProduct.supplierId);
        public const string productId = nameof(SupplierProduct.productId);
        public const string supplyPrice = nameof(SupplierProduct.supplyPrice);
        public const string supplyLeadTime = nameof(SupplierProduct.supplyLeadTime);
    }
    
    public static class Suppliers
    {
        public const string id = nameof(Supplier.id);
        public const string name = nameof(Supplier.name);
        public const string contact = nameof(Supplier.contact);
        public const string email = nameof(Supplier.email);
        public const string phone = nameof(Supplier.phone);
        public const string address = nameof(Supplier.address);
        public const string isActive = nameof(Supplier.isActive);
    }
    
    public static class Branches
    {
        public const string id = nameof(Branch.id);
        public const string name = nameof(Branch.name);
        public const string address = nameof(Branch.address);
        public const string phone = nameof(Branch.phone);
        public const string isActive = nameof(Branch.isActive);
    }
    
    public static class Users
    {
        public const string id = nameof(User.id);
        public const string userName = nameof(User.userName);
        public const string firstName = nameof(User.firstName);
        public const string lastName = nameof(User.lastName);
        public const string email = nameof(User.email);
        public const string password = nameof(User.password);
        public const string isActive = nameof(User.isActive);
    }
    
    public static class Permissions
    {
        public const string id = nameof(Permission.id);
        public const string name = nameof(Permission.name);
        public const string description = nameof(Permission.description);
    }
    
    public static class Roles
    {
        public const string id = nameof(Role.id);
        public const string name = nameof(Role.name);
        public const string description = nameof(Role.description);
        public const string priority = nameof(Role.priority);
    }
    
    public static class Resources
    {
        public const string id = nameof(Resource.id);
        public const string name = nameof(Resource.name);
        public const string description = nameof(Resource.description);
    }
    
    public static class RefreshTokens
    {
        public const string RefreshTokenField = "RefreshToken";
        public const string deviceId = nameof(RefreshToken.deviceId);
    }
    
    public static class UsersRoles
    {
        public const string id = nameof(UserRole.id);
        public const string userId = nameof(UserRole.userId);
        public const string roleId = nameof(UserRole.roleId);
    }
    
    public static class PasswordRecoveryTokens
    {
        public const string id = nameof(PasswordRecoveryToken.id);
        public const string userId = nameof(PasswordRecoveryToken.userId);
        public const string resetToken = nameof(PasswordRecoveryToken.resetToken);
        public const string expiresAt = nameof(PasswordRecoveryToken.expiresAt);
        public const string createdAt = nameof(PasswordRecoveryToken.createdAt);
    }
    
    public static class Brands
    {
        public const string id = nameof(Brand.id);
        public const string name = nameof(Brand.name);
        public const string description = nameof(Brand.description);
        public const string isActive = nameof(Brand.isActive);
    }
    
    public static class Products
    {
        public const string id = nameof(Product.id);
        public const string name = nameof(Product.name);
        public const string barcode = nameof(Product.barcode);
        public const string price = nameof(Product.price);
        public const string description = nameof(Product.description);
        public const string isActive = nameof(Product.isActive);
        public const string brandId = nameof(Product.brandId);
    }
    
    public static class InventoryFields
    {
        public const string id = nameof(InventoryFields.id);
        public const string productId = nameof(InventoryFields.productId);
        public const string quantity = nameof(InventoryFields.quantity);
        public const string isActive = nameof(InventoryFields.isActive);
    }
    
    public static class Examples
    {
        public const string id = nameof(Example.id);
        public const string name = nameof(Example.name);
    }

    public static class Sales
    {
        public const string id = nameof(Sale.id);
    }
}