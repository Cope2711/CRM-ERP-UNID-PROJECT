using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class UpdateSupplierProductDto
{
    [GuidNotEmpty]
    public Guid id { get; set; }
    
    public decimal? supplyPrice { get; set; }
    
    [Range(1, int.MaxValue)]
    public int? supplyLeadTime { get; set; }
}

public class SupplierProductDto
{
    [GuidNotEmpty]
    public Guid id { get; set; }

    [GuidNotEmpty]
    public Guid supplierId { get; set; }

    [GuidNotEmpty]
    public Guid productId { get; set; }

    public decimal? price { get; set; }

    [Range(1, int.MaxValue)]
    public int? supplyLeadTime { get; set; }
    
    public DateTime createdDate { get; set; }

    public DateTime updatedDate { get; set; }
    
    public SupplierDto? supplier { get; set; }

    public ProductDto? product { get; set; }
}

public class SupplierAndProductIdDto
{
    [GuidNotEmpty]
    public Guid SupplierId { get; set; }

    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    
    public decimal? SupplyPrice { get; set; }
    
    public int? SupplyLeadTime { get; set; }
}