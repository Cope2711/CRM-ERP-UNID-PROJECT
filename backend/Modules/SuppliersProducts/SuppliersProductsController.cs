using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/suppliers-products")]
public class SuppliersProductsController(
    ISuppliersProductsQueryService _suppliersProductsQueryService,
    ISuppliersProductsManagementService _suppliersProductsManagementService
) : ControllerBase
{
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "SuppliersProducts")]
    public async Task<ActionResult<SupplierProductDto>> Update([FromBody] UpdateSupplierProductDto updateSupplierProductDto)
    {
        SupplierProduct? supplierProduct = await _suppliersProductsManagementService.Update(updateSupplierProductDto);

        return Ok(supplierProduct.ToDto());
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "SuppliersProducts")]
    public async Task<ActionResult<GetAllResponseDto<SupplierProductDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(SupplierProductDto));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(SupplierProductDto));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(SupplierProductDto));

        GetAllResponseDto<SupplierProduct>
            getAllResponseDto = await _suppliersProductsQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "SuppliersProducts")]
    public async Task<ActionResult<SupplierProductDto>> GetById([FromQuery] Guid id)
    {
        SupplierProduct? supplierProduct = await _suppliersProductsQueryService.GetByIdThrowsNotFound(id);

        return Ok(supplierProduct.ToDto());
    }
    
    [HttpPost("assign-products")]
    [PermissionAuthorize("Assign", "SuppliersProducts")]
    public async Task<ActionResult<ResponsesDto<SupplierAndProductResponseStatusDto>>> AssignProductsToSuppliers(
        [FromBody] SuppliersAndProductsIdsDto suppliersAndProductsIdsDto)
    {
        ResponsesDto<SupplierAndProductResponseStatusDto> responseDto =
            await _suppliersProductsManagementService.AssignProductsToSuppliers(suppliersAndProductsIdsDto);

        return Ok(responseDto);
    }
    
    [HttpDelete("revoke-products")]
    [PermissionAuthorize("Revoke", "SuppliersProducts")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> RevokeProductsFromSuppliers(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> responseDto =
            await _suppliersProductsManagementService.RevokeProductsFromSuppliers(idsDto);

        return Ok(responseDto);
    }
}