using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public abstract class BaseUserRoleDto
{
    [GuidNotEmpty] public Guid UserId { get; set; }
    [MaxLength(50)] public string? UserUserName { get; set; }
    [GuidNotEmpty] public Guid RoleId { get; set; }
    [MaxLength(50)] public string? RoleName { get; set; }
    [MaxLength(255)] public string? RoleDescription { get; set; }
}

public class UserRoleDto : BaseUserRoleDto
{
    [GuidNotEmpty] public Guid UserRoleId { get; set; }
}