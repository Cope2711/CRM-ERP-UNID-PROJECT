using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ProductCategoryDto
{
    [GuidNotEmpty]
    public Guid id { get; set; }
    
    [GuidNotEmpty]
    public Guid productId { get; set; }
    
    [GuidNotEmpty]
    public Guid categoryId { get; set; }
    
    public DateTime createdDate { get; set; }
}

public class ProductAndCategoryIdDto
{
    [GuidNotEmpty]
    public Guid ProductId { get; set; }
    
    [GuidNotEmpty]
    public Guid CategoryId { get; set; }
}