using System.Net.Http.Headers;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Tests;

public class CustomWebApiFactory : WebApplicationFactory<Program>
{
    private string _bearerToken;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();
            
            DatabaseSeeder.Seed(context);
        });
    }

    public HttpClient CreateClientWithBearerToken()
    {
        var client = CreateClient();

        if (_bearerToken == null)
        {
            AuthenticateAsync(client).GetAwaiter().GetResult();
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
        return client;
    }

    private async Task AuthenticateAsync(HttpClient client)
    {
        LoginUserDto loginRequest = new LoginUserDto { UserUserName = Models.Users.Admin.UserUserName, UserPassword = "123456", DeviceId = "devicexd"};
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();    
    
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<TokenDto>();
        _bearerToken = loginResult?.Token;
    }
}