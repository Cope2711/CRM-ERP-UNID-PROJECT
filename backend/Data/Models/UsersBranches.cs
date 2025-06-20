using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("UsersBranches")]
public class UserBranch
{
    [Key]
    public Guid id { get; set; }
    
    [Required]
    public Guid userId { get; set; }
    
    [Required]
    public Guid branchId { get; set; }
    
    public User User { get; set; }
    public Branch Branch { get; set; }
}

public static class UserBranchExtensions
{
    public static UserBranchDto ToDto(this UserBranch userBranch)
    {
        return new UserBranchDto
        {
            id = userBranch.id,
            userId = userBranch.userId,
            branchId = userBranch.branchId
        };
    }
}