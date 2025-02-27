using System.Security.Claims;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRM_ERP_UNID.Attributes;

public class PermissionAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _permission;
    private readonly string? _resource;

    public PermissionAuthorizeAttribute(string permission, string? resource = null) 
    {
        this._permission = permission; 
        this._resource = resource;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user = context.HttpContext.User;

        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedException(message: "User is not authenticated", reason: "UserNotAuthenticated");
        }
        
        IRolesPermissionsResourcesService rolesPermissionsResourcesService =
            context.HttpContext.RequestServices.GetRequiredService<IRolesPermissionsResourcesService>();

        string? roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

        List<Guid> rolesIds = roleClaim.Split(',').Select(Guid.Parse).ToList();

        foreach (Guid roleId in rolesIds)
        {
            if (await rolesPermissionsResourcesService.ArePermissionNameResourceNameAssignedToRoleIdAsync(roleId, _permission, _resource))
            {
                return;
            }
        }

        throw new ForbiddenException(message: $"User has no permission {_permission}", permission: _permission, resource: _resource);
    }
}