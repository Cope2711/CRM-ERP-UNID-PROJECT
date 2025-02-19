using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [MaxLength(50)] [Required] public string UserUserName { get; set; }

    [MaxLength(50)] [Required] public string UserFirstName { get; set; }

    [MaxLength(50)] [Required] public string UserLastName { get; set; }

    [MaxLength(255)]
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }

    [MinLength(6)]
    [MaxLength(255)]
    [Required]
    public string UserPassword { get; set; }

    [Required] public bool IsActive { get; set; } = true;

    [Required] public Guid RoleId { get; set; }
}

public class DeactivateUserDto
{
    [Required] [GuidNotEmpty] public Guid UserId { get; set; }
}

public class UserDto
{
    public Guid UserId { get; set; }

    [MaxLength(50)] public string UserUserName { get; set; }

    [MaxLength(50)] public string UserFirstName { get; set; }

    [MaxLength(50)] public string UserLastName { get; set; }

    [MaxLength(255)] public string UserEmail { get; set; }

    public bool IsActive { get; set; }

    public List<RoleDto> Roles { get; set; }
}

public class UpdateUserDto
{
    [GuidNotEmpty] public Guid UserId { get; set; }
    [MaxLength(50)] public string? UserUserName { get; set; }
    [MaxLength(50)] public string? UserFirstName { get; set; }
    [MaxLength(50)] public string? UserLastName { get; set; }
    [MaxLength(255)] public string? UserEmail { get; set; }
}

public class LoginUserDto
{
    [MaxLength(50)] [Required] public string UserUserName { get; set; }
    [MaxLength(255)] [Required] public string UserPassword { get; set; }
    [MaxLength(255)] [Required] public string DeviceId { get; set; }
}

public class ChangePasswordDto
{
    [Required]
    [MaxLength(255)]
    [MinLength(6)]
    public string ActualPassword { get; set; }

    [Required]
    [MaxLength(255)]
    [MinLength(6)]
    public string NewPassword { get; set; }
}