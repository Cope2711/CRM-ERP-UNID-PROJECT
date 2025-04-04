using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/products-categories")]
public class ProductsCategoriesController(
    IProductsCategoriesManagementService _productsCategoriesManagementService,
    IProductsCategoriesQueryService _productsCategoriesQueryService
) : ControllerBase
{
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "ProductsCategories")]
    public async Task<ActionResult<GetAllResponseDto<ProductCategory>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(ProductCategory));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(ProductCategory));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(ProductCategory));

        GetAllResponseDto<ProductCategory> getAllResponseDto = await _productsCategoriesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpPost("assign")]
    [PermissionAuthorize("Assign", "ProductsCategories")]
    public async Task<ActionResult<ResponsesDto<ProductAndCategoryResponseStatusDto>>> Assign([FromBody] ProductsAndCategoriesDto productsAndCategoriesDto)
    {
        ResponsesDto<ProductAndCategoryResponseStatusDto> responsesDto = await _productsCategoriesManagementService.Assign(productsAndCategoriesDto);

        return Ok(responsesDto);
    }
    
    [HttpDelete("revoke")]
    [PermissionAuthorize("Revoke", "ProductsCategories")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> Revoke([FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> responsesDto = await _productsCategoriesManagementService.Revoke(idsDto);

        return Ok(responsesDto);
    }
}