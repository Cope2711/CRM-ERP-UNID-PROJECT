public static class CorsExtensions
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CustomCorsPolicy", policy =>
            {
                // Permite cualquier puerto de localhost o 127.0.0.1
                policy.SetIsOriginAllowed(origin => 
                    {
                        if (string.IsNullOrWhiteSpace(origin)) return false;
                    
                        try
                        {
                            var uri = new Uri(origin);
                            return uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) 
                                   || uri.Host.Equals("127.0.0.1");
                        }
                        catch
                        {
                            return false;
                        }
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Token-Expired");
            });
        });

        return services;
    }
}