using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules.Products;

public interface IProductsManagementService
{
    Task<Product> Create(CreateProductDto createProductDto);
    Task<Product> Update(UpdateProductDto updateProductDto);
    Task<Product> ChangeBrand(ChangeBrandProductDto changeBrandProductDto);
}