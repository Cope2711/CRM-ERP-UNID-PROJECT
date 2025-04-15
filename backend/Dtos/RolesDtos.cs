using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public abstract class BaseRoleDto
{
    [MaxLength(50)]
    [Required]
    public required string RoleName { get; set; }
    
    [Range(0, 1000)]
    [Required]
    public required double RolePriority { get; set; }
    
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
}

public class CreateRoleDto : BaseRoleDto { }

public class RoleDto : BaseRoleDto
{
    public Guid RoleId { get; set; }
    public List<PermissionDto>? Permissions { get; set; }
}

public class UpdateRoleDto
{
    [MaxLength(50)]
    public string? RoleName { get; set; }
    
    [Range(0, 1000)]
    public double? RolePriority { get; set; }
    
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
}