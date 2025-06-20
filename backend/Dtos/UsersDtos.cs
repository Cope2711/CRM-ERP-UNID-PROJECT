using CRM_ERP_UNID.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Dtos;

public class UserDto
{
    [IsObjectKey]
    [GuidNotEmpty]
    [Required]
    public Guid id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)] 
    public required string userName { get; set; }
    
    [Required]
    [MaxLength(50)] 
    public required string firstName { get; set; }
    
    [Required]
    [MaxLength(50)] 
    public required string lastName { get; set; }
    
    [Required]
    [MaxLength(255)]
    [IsEmail]
    public required string email { get; set; }
    
    [MinLength(6)]
    [MaxLength(255)]
    [IsPassword]
    [Required]
    public string password { get; set; } = string.Empty;

    [Required] 
    public bool isActive { get; set; } = true;
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
