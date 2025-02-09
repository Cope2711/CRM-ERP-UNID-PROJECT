﻿using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Dtos;

public class RolePermissionDto
{
    [GuidNotEmpty(ErrorMessage = "The role id cannot be empty.")]
    public Guid RoleId { get; set; }
    
    [MaxLength(50)]
    public string RoleName { get; set; }
    
    [MaxLength(255)]
    public string? RoleDescription { get; set; }
    
    [GuidNotEmpty(ErrorMessage = "The permission id cannot be empty.")]
    public Guid PermissionId { get; set; }
    
    [MaxLength(100)]
    public string PermissionName { get; set; }
    
    [MaxLength(255)]
    public string? PermissionDescription { get; set; }
}

public class PermissionAndRoleDto
{
    [GuidNotEmpty]
    public Guid RoleId { get; set; }
    
    [GuidNotEmpty]
    public Guid PermissionId { get; set; }
}
