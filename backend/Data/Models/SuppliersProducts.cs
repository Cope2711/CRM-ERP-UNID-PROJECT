using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

[Table("SuppliersProducts")]
public class SupplierProduct
{
    [Key]
    public Guid id { get; set; }
    
    [ForeignKey("supplierId")]
    public Guid supplierId { get; set; }
    
    [ForeignKey("productId")]
    public Guid productId { get; set; }
    
    public decimal? supplyPrice { get; set; }
    
    public int? supplyLeadTime { get; set; }
    
    public DateTime createdDate { get; set; }
    
    public DateTime updatedDate { get; set; }
    
    public Supplier? Supplier { get; set; }
    
    public Product? Product { get; set; }
}

public static class SupplierProductExtensions
{
    public static SupplierProductDto ToDto(this SupplierProduct supplierProduct)
    {
        return new SupplierProductDto
        {
            id = supplierProduct.id,
            supplierId = supplierProduct.supplierId,
            productId = supplierProduct.productId,
            price = supplierProduct.supplyPrice,
            supplyLeadTime = supplierProduct.supplyLeadTime,
            createdDate = supplierProduct.createdDate,
            updatedDate = supplierProduct.updatedDate,
            supplier = supplierProduct.Supplier?.ToDto(),
            product = supplierProduct.Product?.ToDto()
        };
    }
}