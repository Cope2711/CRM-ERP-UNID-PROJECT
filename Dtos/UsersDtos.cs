namespace CRM_ERP_UNID.Dtos;

using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [MaxLength(50)] 
    [Required]
    public string UserUserName { get; set; } 

    [MaxLength(50)] 
    [Required]
    public string UserFirstName { get; set; }

    [MaxLength(50)] 
    [Required]
    public string UserLastName { get; set; } 

    [MaxLength(255)] 
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; } 

    [MinLength(6)]
    [MaxLength(255)] 
    [Required]
    public string UserPassword { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public Guid RoleId { get; set; } //campo de Rol
}


public class LoginUserDto
{
    [MaxLength(50)] 
    [Required]
    public string UserUserName { get; set; }   
    
    [MaxLength(255)] 
    [Required]
    public string UserPassword { get; set; }
}