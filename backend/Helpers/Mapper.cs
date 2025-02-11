using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Helpers;

public static class Mapper
{
    public static RolePermissionResourceDto RolePermissionResourceToRolePermissionResourceDto(RolePermissionResource rolePermissionResource)
    {
        return new RolePermissionResourceDto
        {
            RoleId = rolePermissionResource.RoleId,
            RoleName = rolePermissionResource.Role.RoleName,
            RoleDescription = rolePermissionResource.Role.RoleDescription,
            PermissionId = rolePermissionResource.PermissionId,
            PermissionName = rolePermissionResource.Permission.PermissionName,
            PermissionDescription = rolePermissionResource.Permission.PermissionDescription,
            ResourceId = rolePermissionResource.ResourceId,
            ResourceName = rolePermissionResource.Resource?.ResourceName,
            ResourceDescription = rolePermissionResource.Resource?.ResourceDescription
        };
    }
    
    public static UserDto UserToUserDto(User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            UserUserName = user.UserUserName,
            UserFirstName = user.UserFirstName,
            UserLastName = user.UserLastName,
            UserEmail = user.UserEmail,
            IsActive = user.IsActive,
            Roles = user.UserRoles.Select(ur => new RoleDto
            {
                RoleId = ur.RoleId,
                RoleName = ur.Role.RoleName
            }).ToList()
        };
    }
    
    public static RoleDto RoleToRoleDto(Role role)
    {
        return new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            RoleDescription = role.RoleDescription
        };
    }

    public static UserRoleDto UserRoleToUserRoleDto(UserRole userRole)
    {
        return new UserRoleDto
        {
            UserRoleId = userRole.UserRoleId,
            UserId = userRole.UserId,
            UserUserName = userRole.User.UserUserName,
            RoleId = userRole.RoleId,
            RoleName = userRole.Role.RoleName,
            RoleDescription = userRole.Role.RoleDescription
        };
    }
    
    public static ResourceDto ResourceToResourceDto(Resource resource)
    {
        return new ResourceDto
        {
            ResourceId = resource.ResourceId,
            ResourceName = resource.ResourceName
        };
    }
    
    public static PermissionDto PermissionToPermissionDto(Permission permission)
    {
        return new PermissionDto
        {
            PermissionId = permission.PermissionId,
            PermissionName = permission.PermissionName,
            PermissionDescription = permission.PermissionDescription
        };
    }
}