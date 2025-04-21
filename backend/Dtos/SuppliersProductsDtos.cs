using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class UpdateSupplierProductDto
{
    [GuidNotEmpty]
    public Guid SupplierProductId { get; set; }
    
    public decimal? SupplyPrice { get; set; }
    
    [Range(1, int.MaxValue)]
    public int? SupplyLeadTime { get; set; }
}

public class SupplierProductDto
{
    [GuidNotEmpty]
    public Guid SupplierProductId { get; set; }

    [GuidNotEmpty]
    public Guid SupplierId { get; set; }

    [GuidNotEmpty]
    public Guid ProductId { get; set; }

    public decimal? SupplyPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int? SupplyLeadTime { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }
    
    public SupplierDto? Supplier { get; set; }

    public ProductDto? Product { get; set; }
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