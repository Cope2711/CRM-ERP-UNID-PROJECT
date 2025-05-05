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
    public Guid UserId { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Unique]
    public required string UserUserName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string UserFirstName { get; set; }
    
    [Required]
    [MaxLength(50)]

    public required string UserLastName { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Unique]
    public required string UserEmail { get; set; }
    
    [Required]
    [MaxLength(255)]
    public required string UserPassword { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserBranch> UsersBranches { get; set; } = new List<UserBranch>();
    
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
            }).ToList(),
            Branches = user.UsersBranches.Select(ub => new BranchDto{
                BranchId = ub.BranchId,
                BranchName = ub.Branch.BranchName,
                BranchAddress = ub.Branch.BranchAddress,
                BranchPhone = ub.Branch.BranchPhone,
                IsActive = ub.Branch.IsActive
            }).ToList()
        };
    }
    
    public static double[] ToUserRolesPriority(this User user)
    {
        return user.UserRoles.Select(ur => ur.Role.RolePriority).ToArray();
    }

    public static User ToModel(this CreateUserDto createUserDto)
    {
        return new User
        {
            UserUserName = createUserDto.UserUserName,
            UserFirstName = createUserDto.UserFirstName,
            UserLastName = createUserDto.UserLastName,
            UserEmail = createUserDto.UserEmail,
            UserPassword = HasherHelper.HashString(createUserDto.UserPassword),
            IsActive = createUserDto.IsActive
        };
    }
}