using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("SuppliersProducts")]
public class SupplierProduct
{
    [Key]
    public Guid SupplierProductId { get; set; }
    
    [ForeignKey("SupplierId")]
    public required Guid SupplierId { get; set; }
    
    [ForeignKey("ProductId")]
    public required Guid ProductId { get; set; }
    
    public decimal? SupplyPrice { get; set; }
    
    public int? SupplyLeadTime { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime UpdatedDate { get; set; }
    
    public Supplier? Supplier { get; set; }
    
    public Product? Product { get; set; }
}

public static class SupplierProductExtensions
{
    public static SupplierProductDto ToDto(this SupplierProduct supplierProduct)
    {
        return new SupplierProductDto
        {
            SupplierProductId = supplierProduct.SupplierProductId,
            SupplierId = supplierProduct.SupplierId,
            ProductId = supplierProduct.ProductId,
            SupplyPrice = supplierProduct.SupplyPrice,
            SupplyLeadTime = supplierProduct.SupplyLeadTime,
            CreatedDate = supplierProduct.CreatedDate,
            UpdatedDate = supplierProduct.UpdatedDate,
            Supplier = supplierProduct.Supplier?.ToDto(),
            Product = supplierProduct.Product?.ToDto()
        };
    }
}