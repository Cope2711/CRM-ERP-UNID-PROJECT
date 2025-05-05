using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Branches")]
public class Branch
{
    [Key]
    public Guid BranchId { get; set; }
    
    [Required]
    [StringLength(100)]
    [Unique]
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
    public ICollection<UserBranch> UsersBranches { get; set; } = new List<UserBranch>();
    public ICollection<SupplierBranch> SupplierBranches { get; set; } = new List<SupplierBranch>();
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
            IsActive = branch.IsActive
        };
    }

    public static Branch ToModel(this CreateBranchDto dto)
    {
        return new Branch()
        {
            BranchName = dto.BranchName,
            BranchAddress = dto.BranchAddress,
            BranchPhone = dto.BranchPhone,
            IsActive = dto.IsActive
        };
    }
}