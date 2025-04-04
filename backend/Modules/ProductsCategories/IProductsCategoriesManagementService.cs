using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IProductsCategoriesManagementService
{
    Task<ResponsesDto<ProductAndCategoryResponseStatusDto>> Assign(ProductsAndCategoriesDto productsAndCategoriesDto);
    Task<ResponsesDto<IdResponseStatusDto>> Revoke(IdsDto idsDto);
}