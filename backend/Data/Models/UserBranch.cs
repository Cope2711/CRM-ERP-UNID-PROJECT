using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("UsersBranches")]
public class UserBranch
{
    [Key]
    public Guid UserBranchId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid BranchId { get; set; }
    
    public User User { get; set; }
    public Branch Branch { get; set; }
}

public static class UsersBranchesExtensions
{
    public static UserBranchDto ToDto(this UserBranch userBranch)
    {
        return new UserBranchDto
        {
            UserBranchId = userBranch.UserBranchId,
            UserId = userBranch.UserId,
            BranchId = userBranch.BranchId
        };
    }
}