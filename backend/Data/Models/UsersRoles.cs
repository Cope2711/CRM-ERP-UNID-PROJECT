using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("UsersRoles")]
public class UserRole
{
    [Key]
    public Guid UserRoleId { get; set; }

    [Required]
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Required]
    [ForeignKey("RoleId")]
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
}

public static class UserRoleExtensions
{
    public static UserRoleDto ToDto(this UserRole userRole)
    {
        return new UserRoleDto
        {
            UserRoleId = userRole.UserRoleId,
            UserId = userRole.UserId,
            UserUserName = userRole.User.UserUserName,
            RoleId = userRole.RoleId,
            RoleName = userRole.Role.RoleName,
            RoleDescription = userRole.Role.RoleDescription
        };
    }
}