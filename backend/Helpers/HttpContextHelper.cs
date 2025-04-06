using System.Security.Claims;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Helpers;

public static class HttpContextHelper
{
    public static Guid GetAuthenticatedUserId(IHttpContextAccessor httpContextAccessor)
    {
        return Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                          Guid.Empty.ToString());
    }
    
    public static double GetAuthenticatedUserMaxRolePriority(IHttpContextAccessor httpContextAccessor)
    {
        var maxRolePriorityClaim = httpContextAccessor.HttpContext?.User.FindFirst("MaxRolePriority");
        if (maxRolePriorityClaim == null)
            throw new ForbiddenException("Authenticated user has no roles assigned");
        
        return double.Parse(maxRolePriorityClaim.Value);
    }
}
