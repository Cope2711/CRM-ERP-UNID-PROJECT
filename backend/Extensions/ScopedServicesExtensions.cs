using CRM_ERP_UNID.Modules;
using CRM_ERP_UNID.Modules.Brands;
using CRM_ERP_UNID.Modules.Products;

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
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRolesManagementService, RolesManagementService>();
            services.AddScoped<IRolesQueryService, RolesQueryService>();
            services.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
            services.AddScoped<IUsersRolesService, UsersRolesService>();
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
        }
    }
}