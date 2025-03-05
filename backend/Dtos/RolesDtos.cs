using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class CreateRoleDto
{
    [Required]
    [MaxLength(50)]
    public required string RoleName { get; set; }

    [Required]
    [Range(0, 1000)]
    public required double RolePriority { get; set; }
    
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
}

public class RoleDto
{
    public Guid RoleId { get; set; }
    
    [MaxLength(50)]
    public required string RoleName { get; set; }
    
    [Required]
    [Range(0, 1000)]
    public required double RolePriority { get; set; }
    
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
    public List<PermissionDto>? Permissions { get; set; }
}

public class UpdateRoleDto
{
    [GuidNotEmpty]
    public Guid RoleId { get; set; }
    
    [MaxLength(50)]
    public string? RoleName { get; set; }
    
    [Range(0, 1000)]
    public double? RolePriority { get; set; }
    
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
}