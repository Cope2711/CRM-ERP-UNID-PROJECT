using CRM_ERP_UNID.Extensions;
using Hellang.Middleware.ProblemDetails;
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
builder.Services.AddScopedServices();

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
app.UseCors("CustomCorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseProblemDetails();

app.MapControllers();

app.Run();

public partial class Program
{
}
