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
    public Guid id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Unique]
    public required string name { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string address { get; set; }
    
    [MinLength(9)]
    [MaxLength(20)]
    public string? phone { get; set; }
    
    [NonModificable]
    public bool isActive { get; set; }
    
    [NonModificable]
    public DateTime? createdDate { get; set; }
    
    [NonModificable]
    public DateTime? updatedDate { get; set; }
    
    [NonModificable]
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
    
    [NonModificable]
    [RelationInfo("users", "users-branches", new[] { "id", "User.id", "User.userName" }, "branch.id")]
    public ICollection<UserBranch> UsersBranches { get; set; } = new List<UserBranch>();
    
    [NonModificable]
    [RelationInfo("suppliers", "suppliers-branches", new[] { "id", "Supplier.id", "Supplier.name" }, "branch.id")]
    public ICollection<SupplierBranch> SupplierBranches { get; set; } = new List<SupplierBranch>();
}

public static class BranchExtensions
{
    public static BranchDto ToDto(this Branch branch)
    {
        return new BranchDto
        {
            id = branch.id,
            name = branch.name,
            address = branch.address,
            phone = branch.phone,
            isActive = branch.isActive
        };
    }
}