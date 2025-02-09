using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Helpers;

public static class Mapper
{
    public static RolePermissionDto RolePermissionToRolePermissionDto(RolePermission rolePermission)
    {
        return new RolePermissionDto
        {
            RoleId = rolePermission.RoleId,
            RoleName = rolePermission.Role.RoleName,
            RoleDescription = rolePermission.Role.RoleDescription,
            PermissionId = rolePermission.PermissionId,
            PermissionName = rolePermission.Permission.PermissionName,
            PermissionDescription = rolePermission.Permission.PermissionDescription
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
    
    public static PermissionResourceDto PermissionResourceToPermissionResourceDto(PermissionResource permissionResource)
    {
        return new PermissionResourceDto
        {
            PermissionResourceId = permissionResource.PermissionResourceId,
            PermissionId = permissionResource.PermissionId,
            PermissionName = permissionResource.Permission.PermissionName,
            PermissionDescription = permissionResource.Permission.PermissionDescription,
            ResourceId = permissionResource.ResourceId,
            ResourceName = permissionResource.Resource.ResourceName
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