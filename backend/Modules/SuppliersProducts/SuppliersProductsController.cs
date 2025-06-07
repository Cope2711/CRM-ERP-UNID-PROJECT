using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
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
    
    [HttpPost("assign")]
    [PermissionAuthorize("Assign", "SuppliersProducts")]
    public async Task<ActionResult<ResponsesDto<ModelAndAssignResponseStatusDto>>> AssignProductsToSuppliers(
        [FromBody] ModelsAndAssignsDtos modelsAndAssignsDtos, [FromQuery] string? modelName)
    {
        // Change the model and assigns ids depending on the frontend context
        if (modelName != null && modelName.ToLower() == "suppliers")
        {
            foreach (var assign in modelsAndAssignsDtos.ModelAssignIds)
            {
                var temp = assign.ModelId;
                assign.ModelId = assign.AssignId;
                assign.AssignId = temp;
            }
        }
        
        ResponsesDto<ModelAndAssignResponseStatusDto> responseDto =
            await _suppliersProductsManagementService.AssignProductsToSuppliers(modelsAndAssignsDtos);

        return Ok(responseDto);
    }
    
    [HttpDelete("revoke")]
    [PermissionAuthorize("Revoke", "SuppliersProducts")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> RevokeProductsFromSuppliers(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> responseDto =
            await _suppliersProductsManagementService.RevokeProductsFromSuppliers(idsDto);

        return Ok(responseDto);
    }
}