using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Data.Models;

[Table("Users")]
public class User
{
    [Key]
    [NonModificable]
    public Guid id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Unique]
    public string userName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string firstName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]

    public string lastName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    [Unique]
    public string email { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(255)]
    [IsPassword]
    public string password { get; set; }
    
    [NonModificable]
    public bool isActive { get; set; }
    
    [NonModificable]
    [RelationInfo("roles", "users-roles", new[] { "id", "Role.id", "Role.name", "Role.priority" }, "user.id")]
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    [NonModificable]
    [RelationInfo("branches", "users-branches", new[] { "id", "Branch.id", "Branch.name" }, "user.id")]
    public ICollection<UserBranch> UsersBranches { get; set; } = new List<UserBranch>();
    
    [NonModificable]
    public DateTime? createdDate { get; set; }
    
    [NonModificable]
    public DateTime? updatedDate { get; set; } 
}

public static class UserExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto  
        {
            id = user.id,
            userName = user.userName,
            firstName = user.firstName,
            lastName = user.lastName,
            email = user.email,
            isActive = user.isActive
        };
    }
    
    public static double[] ToUserRolesPriority(this User user)
    {
        return user.UserRoles.Select(ur => ur.Role.priority).ToArray();
    }
}