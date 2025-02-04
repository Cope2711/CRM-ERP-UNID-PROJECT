using CRM_ERP_UNID.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddCustomDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (environment.IsEnvironment("Test"))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        return services;
    }
}