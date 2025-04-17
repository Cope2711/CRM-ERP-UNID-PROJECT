using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class SupplierDto
{
    [IsObjectKey]
    [GuidNotEmpty]
    [Required]
    public Guid SupplierId { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string SupplierName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? SupplierContact { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    [IsEmail]
    public string? SupplierEmail { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    [IsPhoneNumberWithLada]
    public string? SupplierPhone { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? SupplierAddress { get; set; }
    
    [Required] 
    public bool IsActive { get; set; } = true;
    
    [RelationInfo("SuppliersProducts", "suppliers-products", new[] { "Product.productName" })]
    public List<SupplierProductDto> Products { get; set; } = new();
    
    [RelationInfo("SuppliersBranches", "suppliers-branches", new[] { "Branch.branchName" })]
    public List<SupplierBranchDto> Branches { get; set; } = new();
}

public class CreateSupplierDto
{
    [Required]
    [MaxLength(100)]
    public required string SupplierName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string? SupplierContact { get; set; }
    
    [Required]
    [MaxLength(100)]
    [IsEmail]
    public required string SupplierEmail { get; set; }
    
    [MaxLength(20)]
    [IsPhoneNumberWithLada]
    public string? SupplierPhone { get; set; }
    
    [MaxLength(255)]
    public string? SupplierAddress { get; set; }
    
    [Required] 
    public bool IsActive { get; set; } = true;
}

public class UpdateSupplierDto
{
    [MinLength(3)]
    [MaxLength(50)]
    public string? SupplierName { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? SupplierContact { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    [IsEmail]
    public string? SupplierEmail { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    [IsPhoneNumberWithLada]
    public string? SupplierPhone { get; set; }
    
    [MinLength(3)]
    [MaxLength(255)]
    public string? SupplierAddress { get; set; }
    
    public bool IsActive { get; set; }
}