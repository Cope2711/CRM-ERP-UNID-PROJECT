using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Suppliers")]
public class Supplier
{
    [Key]
    [NonModificable]
    public Guid SupplierId { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(100)]
    [Unique]
    public required string SupplierName { get; set; }
    
    [MinLength(4)]
    [MaxLength(50)]
    public string? SupplierContact { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(100)]
    [Unique]
    public required string SupplierEmail { get; set; }
    
    [MinLength(8)]
    [MaxLength(20)]
    public string? SupplierPhone { get; set; }
    
    [MinLength(4)]
    [MaxLength(255)]
    public string? SupplierAddress { get; set; }
    
    [NonModificable]
    public bool IsActive { get; set; }
    
    [NonModificable]
    public DateTime? CreatedDate { get; set; }
    [NonModificable]
    public DateTime? UpdatedDate { get; set; }
    
    [NonModificable]
    [RelationInfo("products", "suppliers-products", new[] { "SupplierProductId", "Product.ProductId", "Product.ProductName" })]
    public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    
    [NonModificable]
    [RelationInfo("branches", "suppliers-branches", new[] { "SupplierBranchId", "Branch.BranchId", "Branch.BranchName" })]
    public ICollection<SupplierBranch> SupplierBranches { get; set; } = new List<SupplierBranch>();
}

public static class SupplierExtensions
{
    public static SupplierDto ToDto(this Supplier supplier)
    {
        return new SupplierDto
        {
            SupplierId = supplier.SupplierId,
            SupplierName = supplier.SupplierName,
            SupplierContact = supplier.SupplierContact,
            SupplierEmail = supplier.SupplierEmail,
            SupplierPhone = supplier.SupplierPhone,
            SupplierAddress = supplier.SupplierAddress,
            IsActive = supplier.IsActive,
        };
    }

    public static Supplier ToModel(this CreateSupplierDto dto)
    {
        return new Supplier()
        {
            SupplierName = dto.SupplierName,
            SupplierContact = dto.SupplierContact,
            SupplierEmail = dto.SupplierEmail,
            SupplierPhone = dto.SupplierPhone,
            SupplierAddress = dto.SupplierAddress,
            IsActive = dto.IsActive
        };
    }
}