using CRM_ERP_UNID.Extensions;
using CRM_ERP_UNID.Modules;
using CRM_ERP_UNID.Modules.RecoverPassword;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.Test.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    builder.Host.UseSerilog();
}

// Add services to the container.
builder.Services.AddScoped<IRolesPermissionsResourcesRepository, RolesPermissionsResourcesResourcesRepository>();
builder.Services.AddScoped<IRolesPermissionsResourcesService, RolesPermissionsResourcesService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITokensRepository, TokensRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUsersRolesRepository, UsersRolesRepository>();
builder.Services.AddScoped<IUsersRolesService, UsersRolesService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped(typeof(IGenericServie<>), typeof(GenericService<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
builder.Services.AddScoped<PasswordResetService>();



builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCustomDatabaseConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddCustomProblemDetails(builder.Environment);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomRateLimiting();
builder.Services.AddCustomCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseProblemDetails();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();

public partial class Program
{
}