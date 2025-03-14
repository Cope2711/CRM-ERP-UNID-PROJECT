using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Users")]
public class User
{
    [Key]
    public Guid UserId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UserUserName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UserFirstName { get; set; }
    
    [Required]
    [MaxLength(50)]

    public string UserLastName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string UserEmail { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string UserPassword { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; } 
}

public static class UserExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto  
        {
            UserId = user.UserId,
            UserUserName = user.UserUserName,
            UserFirstName = user.UserFirstName,
            UserLastName = user.UserLastName,
            UserEmail = user.UserEmail,
            IsActive = user.IsActive,
            Roles = user.UserRoles.Select(ur => new RoleDto{
                RoleId = ur.RoleId,
                RolePriority = ur.Role.RolePriority,
                RoleName = ur.Role.RoleName
            }).ToList()
        };
    }
    
    public static double[] ToUserRolesPriority(this User user)
    {
        return user.UserRoles.Select(ur => ur.Role.RolePriority).ToArray();
    }
}