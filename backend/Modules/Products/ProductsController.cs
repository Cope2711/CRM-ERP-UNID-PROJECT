using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/products")]
public class ProductsController(
    IProductsManagementService _productsManagementService,
    IProductsQueryService _productsQueryService
) : ControllerBase
{
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Products")]
    public async Task<ActionResult<GetAllResponseDto<Product>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Product));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Product));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(Product));

        GetAllResponseDto<Product> getAllResponseDto = await _productsQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Products")]
    public async Task<ActionResult<ProductDto>> GetProductById([FromQuery] Guid id)
    {
        Product product = await _productsQueryService.GetByIdThrowsNotFound(id);

        return Ok(product.ToDto());
    }

    [HttpGet("get-by-name")]
    [PermissionAuthorize("View", "Products")]
    public async Task<ActionResult<ProductDto>> GetProductByName([FromQuery] string name)
    {
        Product product = await _productsQueryService.GetByNameThrowsNotFound(name);

        return Ok(product.ToDto());
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Products")]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto createProductDto)
    {
        Product product = await _productsManagementService.Create(createProductDto);

        return Ok(product.ToDto());
    }
    
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Products")]
    public async Task<ActionResult<ProductDto>> Update([FromBody] UpdateProductDto updateProductDto)
    {
        Product product = await _productsManagementService.Update(updateProductDto);

        return Ok(product.ToDto());
    }
    
    [HttpPatch("change-brand")]
    [PermissionAuthorize("Edit_Content", "Products")]
    public async Task<ActionResult<ProductDto>> ChangeBrand([FromBody] ChangeBrandProductDto changeBrandProductDto)
    {
        Product product = await _productsManagementService.ChangeBrand(changeBrandProductDto);

        return Ok(product.ToDto());
    }
}