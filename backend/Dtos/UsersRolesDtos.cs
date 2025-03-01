using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class UserRoleDto
{
    [GuidNotEmpty] public Guid UserRoleId { get; set; }

    [GuidNotEmpty] public Guid UserId { get; set; }

    [MaxLength(50)] public string UserUserName { get; set; }

    [GuidNotEmpty] public Guid RoleId { get; set; }

    [MaxLength(50)] public string RoleName { get; set; }

    [MaxLength(255)] public string? RoleDescription { get; set; }
}

public class UsersAndRolesDtos
{
    [Required]
    [RangeListLength(1, 50)] 
    public required List<UserAndRoleIdDto> UserAndRoleId { get; set; }
}

public class UserAndRoleIdDto
{
    [Required] [GuidNotEmpty] public required Guid UserId { get; set; }

    [Required] [GuidNotEmpty] public required Guid RoleId { get; set; }
}

public class UserAndRoleResponseStatusDto : ResponseStatusDto
{
    public required UserAndRoleIdDto UserAndRoleId { get; set; }
}