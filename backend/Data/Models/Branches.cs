using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Branches")]
public class Branch
{
    [Key]
    [NonModificable]
    public Guid BranchId { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Unique]
    public required string BranchName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string BranchAddress { get; set; }
    
    [MinLength(9)]
    [MaxLength(11)]
    public string? BranchPhone { get; set; }
    
    [NonModificable]
    public bool IsActive { get; set; }
    [NonModificable]
    public DateTime? CreatedDate { get; set; }
    [NonModificable]
    public DateTime? UpdatedDate { get; set; }
    
    [NonModificable]
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    
    [NonModificable]
    [RelationInfo("users", "users-branches", new[] { "UserBranchId", "User.UserId", "User.UserUserName" })]
    public ICollection<UserBranch> UsersBranches { get; set; } = new List<UserBranch>();
    
    [NonModificable]
    [RelationInfo("suppliers", "suppliers-branches", new[] { "SupplierBranchId", "Supplier.SupplierId", "Supplier.SupplierName" })]
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