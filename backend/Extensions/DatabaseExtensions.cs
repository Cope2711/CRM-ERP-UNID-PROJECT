using CRM_ERP_UNID.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddCustomDatabaseConfiguration(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (environment.IsEnvironment("Test"))
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connection); });

            using (var serviceProvider = services.BuildServiceProvider())
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                options.EnableSensitiveDataLogging(false);
                options.LogTo(_ => { }, LogLevel.None);
            });
        }

        return services;
    }
}