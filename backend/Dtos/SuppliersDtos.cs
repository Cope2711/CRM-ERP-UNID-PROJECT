using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class SupplierDto
{
    [GuidNotEmpty]
    public Guid SupplierId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string SupplierName { get; set; }
    
    [MaxLength(50)]
    public string? SupplierContact { get; set; }
    
    [MaxLength(100)]
    public string? SupplierEmail { get; set; }
    
    [MaxLength(20)]
    public string? SupplierPhone { get; set; }
    
    [MaxLength(255)]
    public string? SupplierAddress { get; set; }
    
    [Required]
    public required bool IsActive { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
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
    public required bool IsActive { get; set; }
}

public class UpdateSupplierDto
{
    [MaxLength(100)]
    public string? SupplierName { get; set; }
    
    [MaxLength(50)]
    public string? SupplierContact { get; set; }
    
    [MaxLength(100)]
    public string? SupplierEmail { get; set; }
    
    [MaxLength(20)]
    public string? SupplierPhone { get; set; }
    
    [MaxLength(255)]
    public string? SupplierAddress { get; set; }
    
    public bool? IsActive { get; set; }
}