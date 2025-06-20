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
    public Guid id { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(100)]
    [Unique]
    public string name { get; set; }
    
    [MinLength(4)]
    [MaxLength(50)]
    public string? contact { get; set; }
    
    [Required]
    [MinLength(4)]
    [MaxLength(100)]
    [Unique]
    public string email { get; set; }
    
    [MinLength(8)]
    [MaxLength(20)]
    public string? phone { get; set; }
    
    [MinLength(4)]
    [MaxLength(255)]
    public string? address { get; set; }
    
    [NonModificable]
    public bool isActive { get; set; }
    
    [NonModificable]
    public DateTime? createdDate { get; set; }
    [NonModificable]
    public DateTime? updatedDate { get; set; }
    
    [NonModificable]
    [RelationInfo("products", "suppliers-products", new[] { "id", "Product.id", "Product.name" }, "supplier.id")]
    public ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    
    [NonModificable]
    [RelationInfo("branches", "suppliers-branches", new[] { "id", "Branch.id", "Branch.name" }, "supplier.id")]
    public ICollection<SupplierBranch> SupplierBranches { get; set; } = new List<SupplierBranch>();
}

public static class SupplierExtensions
{
    public static SupplierDto ToDto(this Supplier supplier)
    {
        return new SupplierDto
        {
            id = supplier.id,
            name = supplier.name,
            contact = supplier.contact,
            email = supplier.email,
            phone = supplier.phone,
            address = supplier.address,
            isActive = supplier.isActive,
        };
    }
}