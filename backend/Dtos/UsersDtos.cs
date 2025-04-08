using CRM_ERP_UNID.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Dtos;

public abstract class BaseUserDto
{
    [MaxLength(50)] public string? UserUserName { get; set; }
    [MaxLength(50)] public string? UserFirstName { get; set; }
    [MaxLength(50)] public string? UserLastName { get; set; }
    [MaxLength(255)] public string? UserEmail { get; set; }
}

public abstract class RequiredBaseUserDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)] 
    public required string UserUserName { get; set; }
    
    [Required]
    [MaxLength(50)] 
    public required string UserFirstName { get; set; }
    
    [Required]
    [MaxLength(50)] 
    public required string UserLastName { get; set; }
    
    [Required]
    [MaxLength(255)]
    [IsEmail]
    public required string UserEmail { get; set; }
}


public class CreateUserDto : RequiredBaseUserDto
{
    [MinLength(6)]
    [MaxLength(255)]
    [Required]
    public string UserPassword { get; set; } = string.Empty;

    [Required] 
    public bool IsActive { get; set; } = true;
}

public class UpdateUserDto : BaseUserDto
{
    [GuidNotEmpty] public Guid UserId { get; set; }
}

public class UserDto : RequiredBaseUserDto
{
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
    public List<RoleDto> Roles { get; set; } = new();
    public List<BranchDto> Branches { get; set; } = new();
}

public class LoginUserDto
{
    [Required] [MaxLength(50)] public string UserUserName { get; set; }
    [Required] [MaxLength(255)] public string UserPassword { get; set; }
    [Required] [MaxLength(255)] public string DeviceId { get; set; }
}

public class ChangePasswordDto
{
    [Required] [MaxLength(255)] [MinLength(6)]
    public string ActualPassword { get; set; }

    [Required] [MaxLength(255)] [MinLength(6)]
    public string NewPassword { get; set; }
}

public class UsersIdsDto
{
    [GuidNotEmpty]
    [RangeListLength(1, 15)]
    public required List<Guid> Ids { get; set; }
}


public class UserResponseStatusDto : ResponseStatusDto
{
    public required Guid UserId { get; set; }
}
