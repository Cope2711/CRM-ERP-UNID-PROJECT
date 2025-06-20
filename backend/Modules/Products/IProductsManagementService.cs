using System.Text.Json;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IProductsManagementService
{
    Task<Product> Create(Product data);
    Task<Product> Update(Guid id, JsonElement data);
    Task<Product> ChangeBrand(ChangeBrandProductDto changeBrandProductDto);
    Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto);
    Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto);
}