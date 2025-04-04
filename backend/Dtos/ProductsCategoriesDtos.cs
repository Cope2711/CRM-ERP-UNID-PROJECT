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

public class ProductsAndCategoriesDto
{
    [RangeListLength(1, int.MaxValue)]
    public required List<ProductAndCategoryIdDto> ProductAndCategoryIdDto { get; set; }
}

public class ProductAndCategoryResponseStatusDto : ResponseStatusDto
{
    public ProductAndCategoryIdDto ProductAndCategoryId { get; set; }
}