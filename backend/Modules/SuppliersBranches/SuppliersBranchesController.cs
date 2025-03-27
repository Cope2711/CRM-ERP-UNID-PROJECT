using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/suppliers-branches")]
public class SuppliersBranchesController(
    ISuppliersBranchesManagementService _suppliersBranchesManagementService,
    ISuppliersBranchesQueryService _suppliersBranchesQueryService
) : ControllerBase
{
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "SuppliersBranches")]
    public async Task<ActionResult<SupplierBranchDto>> Update([FromBody] UpdateSupplierBranchDto updateSupplierBranchDto)
    {
        SupplierBranch? supplierBranch = await _suppliersBranchesManagementService.Update(updateSupplierBranchDto);

        return Ok(supplierBranch.ToDto());
    }
    
    [HttpPost("assign-branches")]
    [PermissionAuthorize("Assign_Supplier_Branch")]
    public async Task<ActionResult<ResponsesDto<SuppliersBranchResponseStatusDto>>> AssignBranchesToSuppliers(
        [FromBody] SuppliersAndBranchesDto suppliersAndBranchesDto)
    {
        ResponsesDto<SuppliersBranchResponseStatusDto> responseDto =
            await _suppliersBranchesManagementService.AssignBranchesToSuppliers(suppliersAndBranchesDto);

        return Ok(responseDto);
    }
    
    [HttpPost("revoke-branches")]
    [PermissionAuthorize("Revoke_Supplier_Branch")]
    public async Task<ActionResult<ResponsesDto<SuppliersBranchesRevokedResponseStatusDto>>> RevokeBranchesFromSuppliers(
        [FromBody] SuppliersBranchesIdsDto suppliersBranchesIdsDto)
    {
        ResponsesDto<SuppliersBranchesRevokedResponseStatusDto> responseDto =
            await _suppliersBranchesManagementService.RevokeBranchesFromSuppliers(suppliersBranchesIdsDto);

        return Ok(responseDto);
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "SuppliersBranches")]
    public async Task<ActionResult<GetAllResponseDto<SupplierBranch>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(SupplierBranch));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(SupplierBranch));

        GetAllResponseDto<SupplierBranch> getAllResponseDto = await _suppliersBranchesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "SuppliersBranches")]
    public async Task<ActionResult<SupplierBranchDto>> GetById([FromQuery] Guid id)
    {
        SupplierBranch? supplierBranch = await _suppliersBranchesQueryService.GetByIdThrowsNotFound(id);

        return Ok(supplierBranch.ToDto());
    }
}