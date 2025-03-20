using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Branches")]
public class Branch
{
    [Key]
    public Guid BranchId { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string BranchName { get; set; }
    
    [Required]
    [StringLength(255)]
    public required string BranchAddress { get; set; }
    
    [StringLength(20)]
    public string? BranchPhone { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}

public static class BranchExtensions
{
    public static BranchDto ToDto(this Branch branch)
    {
        return new BranchDto
        {
            BranchId = branch.BranchId,
            BranchName = branch.BranchName,
            BranchAddress = branch.BranchAddress,
            BranchPhone = branch.BranchPhone,
            IsActive = branch.IsActive,
            CreatedDate = branch.CreatedDate,
            UpdatedDate = branch.UpdatedDate
        };
    }
}