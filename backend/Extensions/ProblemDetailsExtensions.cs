using CRM_ERP_UNID.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection serviceCollection, IWebHostEnvironment webHostEnvironment)
    {
        serviceCollection.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (ctx, ex) => webHostEnvironment.IsDevelopment();

            options.Map<NotFoundException>(ex => new ProblemDetails
            {
                Title = "Resource Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message,
                Extensions = { { "field", ex.Field } }
            });

            options.Map<UniqueConstraintViolationException>(ex => new ProblemDetails
            {
                Title = "Unique Constraint Violation",
                Status = StatusCodes.Status409Conflict,
                Detail = ex.Message,
                Extensions = { { "field", ex.Field } }
            });

            options.Map<UnauthorizedException>(ex => new ProblemDetails
            {
                Title = "Unauthorized",
                Status = StatusCodes.Status401Unauthorized,
                Detail = ex.Message,
                Extensions = { { "reason", ex.Reason } }
            });

            options.Map<ForbiddenException>(ex => new ProblemDetails
            {
                Title = "Forbidden",
                Status = StatusCodes.Status403Forbidden,
                Detail = ex.Message,
                Extensions = { { "permission", ex.Permission }, { "resource", ex.Resource } }
            });
            
            options.Map<BadRequestException>(ex => new ProblemDetails
            {
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
                Extensions = { { "field", ex.Field } }
            });
            
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });

        serviceCollection.AddControllersWithViews()
            .AddProblemDetailsConventions()
            .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

        return serviceCollection;
    }
}