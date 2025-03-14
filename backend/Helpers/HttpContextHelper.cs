using System.Security.Claims;

namespace CRM_ERP_UNID.Helpers;

public static class HttpContextHelper
{
    public static Guid GetAuthenticatedUserId(IHttpContextAccessor httpContextAccessor)
    {
        return Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                          Guid.Empty.ToString());
    }
    
    public static double[] GetAuthenticatedUserRolePriorities(IHttpContextAccessor httpContextAccessor)
    {
        var prioritiesClaim = httpContextAccessor.HttpContext?.User.FindFirst("RolePriorities")?.Value;

        if (string.IsNullOrEmpty(prioritiesClaim))
        {
            return Array.Empty<double>();
        }

        return prioritiesClaim.Split(',')
            .Select(p => double.TryParse(p, out var result) ? result : (double?)null)
            .Where(p => p.HasValue)
            .Select(p => p.Value)
            .ToArray();
    }
}
