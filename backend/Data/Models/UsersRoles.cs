using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("UsersRoles")]
public class UserRole
{
    [Key]
    public Guid id { get; set; }

    [Required]
    [ForeignKey("userId")]
    public Guid userId { get; set; }
    public User User { get; set; }

    [Required]
    [ForeignKey("roleId")]
    public Guid roleId { get; set; }
    public Role Role { get; set; }
}

public static class UserRoleExtensions
{
    public static UserRoleDto ToDto(this UserRole userRole)
    {
        return new UserRoleDto
        {
            id = userRole.id,
            userId = userRole.userId,
            userName = userRole.User.userName,
            roleId = userRole.roleId,
            roleName = userRole.Role.name,
            roleDescription = userRole.Role.description
        };
    }
}