using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public abstract class BaseUserRoleDto
{
    [GuidNotEmpty] public Guid userId { get; set; }
    [MaxLength(50)] public string? userName { get; set; }
    [GuidNotEmpty] public Guid roleId { get; set; }
    [MaxLength(50)] public string? roleName { get; set; }
    [MaxLength(255)] public string? roleDescription { get; set; }
}

public class UserRoleDto : BaseUserRoleDto
{
    [GuidNotEmpty] public Guid id { get; set; }
}