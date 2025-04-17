using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("Suppliers")]
public class Supplier
{
    [Key]
    public Guid SupplierId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string SupplierName { get; set; }
    
    [MaxLength(50)]
    public string? SupplierContact { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string SupplierEmail { get; set; }
    
    [MaxLength(20)]
    public string? SupplierPhone { get; set; }
    
    [MaxLength(255)]
    public string? SupplierAddress { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
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
}