using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class BranchDto
{
    [Required]
    [IsObjectKey]
    [GuidNotEmpty]
    public Guid BranchId { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string BranchName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string BranchAddress { get; set; }
    
    [Required]
    [IsPhoneNumberWithLada]
    public string? BranchPhone { get; set; }
    
    public required bool IsActive { get; set; }
    
    [RelationInfo("SuppliersBranches", "suppliers-branches", new[] { "SupplierBranchId", "Supplier.SupplierId", "Supplier.SupplierName" })]
    public List<SupplierDto> Suppliers { get; set; } = new List<SupplierDto>();
    
    [RelationInfo("UsersBranches", "users-branches", new[] { "UserBranchId", "User.UserId", "User.UserUserName" })]
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}

public class CreateBranchDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string BranchName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string BranchAddress { get; set; }
    
    [Required]
    [IsPhoneNumberWithLada]
    public required string BranchPhone { get; set; }

    [Required] 
    public bool IsActive { get; set; } = true;
}

public class UpdateBranchDto
{
    [MinLength(3)]
    [MaxLength(50)]
    public string? BranchName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? BranchAddress { get; set; }
    
    [IsPhoneNumberWithLada]
    public string? BranchPhone { get; set; }
    public bool IsActive { get; set; }
}