using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IProductsManagementService
{
    Task<Product> Create(CreateProductDto createProductDto);
    Task<Product> Update(Guid id, UpdateProductDto updateProductDto);
    Task<Product> ChangeBrand(ChangeBrandProductDto changeBrandProductDto);
    Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto);
    Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto);
}