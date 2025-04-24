using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ProductCategoryDto
{
    [GuidNotEmpty]
    public Guid ProductCategoryId { get; set; }
    
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    
    [GuidNotEmpty]
    public Guid CategoryId { get; set; }
    
    public DateTime CreatedDate { get; set; }
}

public class ProductAndCategoryIdDto
{
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    
    [GuidNotEmpty]
    public Guid CategoryId { get; set; }
}