using System.Text.Json;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
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
    [HttpGet("schema")]
    [PermissionAuthorize("View", "Users")]
    public IActionResult GetSchema([FromQuery] bool ignoreRequired = false)
    {
        return Ok(DtoSchemaHelper.GetDtoSchema(typeof(Product), ignoreRequired));
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Products")]
    public async Task<ActionResult<GetAllResponseDto<Product>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        GetAllResponseDto<Product> getAllResponseDto = await _productsQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Products")]
    public async Task<ActionResult<ProductDto>> GetById([FromQuery] Guid id)
    {
        Product product = await _productsQueryService.GetByIdThrowsNotFound(id);

        return Ok(product.ToDto());
    }
    
    [HttpGet("get-by-barcode")]
    [PermissionAuthorize("View", "Products")]
    public async Task<ActionResult<ProductDto>> GetByBarcode([FromQuery] string barcode)
    {
        Product product = await _productsQueryService.GetByBarcodeThrowsNotFound(barcode);

        return Ok(product.ToDto());
    }

    [HttpGet("get-by-name")]
    [PermissionAuthorize("View", "Products")]
    public async Task<ActionResult<ProductDto>> GetByName([FromQuery] string name)
    {
        Product product = await _productsQueryService.GetByNameThrowsNotFound(name);

        return Ok(product.ToDto());
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Products")]
    public async Task<ActionResult<ProductDto>> Create([FromBody] Product data)
    {
        Product product = await _productsManagementService.Create(data);

        return Ok(product.ToDto());
    }
    
    [HttpPatch("update/{id}")]
    [PermissionAuthorize("Edit_Content", "Products")]
    public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] JsonElement jsonData)
    {   
        Product product = await _productsManagementService.Update(id, jsonData);

        return Ok(product.ToDto());
    }
    
    [HttpPatch("change-brand")]
    [PermissionAuthorize("Edit_Content", "Products")]
    public async Task<ActionResult<ProductDto>> ChangeBrand([FromBody] ChangeBrandProductDto changeBrandProductDto)
    {
        Product product = await _productsManagementService.ChangeBrand(changeBrandProductDto);

        return Ok(product.ToDto());
    }
    
    [HttpPatch("deactivate")]
    [PermissionAuthorize("Deactivate", "Products")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> Deactivate(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> deactivateResponseDto =
            await _productsManagementService.Deactivate(idsDto);
        return Ok(deactivateResponseDto);
    }

    [HttpPatch("activate")]
    [PermissionAuthorize("Activate", "Products")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> Activate(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> activateResponseDto =
            await _productsManagementService.Activate(idsDto);
        return Ok(activateResponseDto);
    }
}