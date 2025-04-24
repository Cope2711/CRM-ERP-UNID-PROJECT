using CRM_ERP_UNID.Modules;

namespace CRM_ERP_UNID.Extensions
{
    public static class ScopedServicesExtensions
    {
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IRolesPermissionsResourcesRepository, RolesPermissionsResourcesRepository>();
            services.AddScoped<IRolesPermissionsResourcesService, RolesPermissionsResourcesService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokensRepository, TokensRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsersManagementService, UsersManagementService>();
            services.AddScoped<IUsersQueryService, UsersQueryService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRolesManagementService, RolesManagementService>();
            services.AddScoped<IRolesQueryService, RolesQueryService>();
            services.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
            services.AddScoped<IUsersRolesManagementService, UsersRolesManagementService>();
            services.AddScoped<IUsersRolesQueryService, UsersRolesQueryService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IPriorityValidationService, PriorityValidationService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();
            services.AddScoped<IProductsManagementService, ProductsManagementService>();
            services.AddScoped<IProductsQueryService, ProductsQueryService>();
            services.AddScoped<IBrandsService, BrandsService>();
            services.AddScoped<IBrandsRepository, BrandsRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IInventoryManagementService, InventoryManagementService>();
            services.AddScoped<IInventoryQueryService, InventoryQueryService>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IBranchesRepository, BranchesRepository>();
            services.AddScoped<IBranchesQueryService, BranchesQueryService>();
            services.AddScoped<IBranchesManagementService, BranchesManagementService>();
            services.AddScoped<IUsersBranchesRepository, UsersBranchesRepository>();
            services.AddScoped<IUsersBranchesQueryService, UsersBranchesQueryServices>();
            services.AddScoped<IUsersBranchesManagementService, UsersBranchesManagementService>();
            services.AddScoped<ISuppliersManagementService, SuppliersManagementService>();
            services.AddScoped<ISuppliersQueryService, SuppliersQueryServices>();
            services.AddScoped<ISuppliersRepository, SuppliersRepository>();
            services.AddScoped<ISuppliersProductsQueryService, SuppliersProductsQueryService>();
            services.AddScoped<ISuppliersProductsManagementService, SuppliersProductsManagementService>();
            services.AddScoped<ISuppliersProductsRepository, SuppliersProductsRepository>();
            services.AddScoped<ISuppliersBranchesManagementService, SuppliersBranchesManagementService>();
            services.AddScoped<ISuppliersBranchesQueryService, SuppliersBranchesQueryService>();
            services.AddScoped<ISuppliersBranchesRepository, SuppliersBranchesRepository>();
            services.AddScoped<ICategoriesManagementService, CategoriesManagementService>();
            services.AddScoped<ICategoriesQueryService, CategoriesQueryService>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IProductsCategoriesManagementService, ProductsCategoriesManagementService>();
            services.AddScoped<IProductsCategoriesQueryService, ProductsCategoriesQueryService>();
            services.AddScoped<IProductsCategoriesRepository, ProductsCategoriesRepository>();
            services.AddScoped<ISalesQueryService, SalesQueryService>();
            services.AddScoped<ISalesManagementService, SalesManagementService>();
            services.AddScoped<ISalesRepository, SalesRepository>();
        }
    }
}