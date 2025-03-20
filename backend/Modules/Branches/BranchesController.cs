using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/branches")]
public class Branches(
    IBranchesQueryService _branchesQueryService,
    IBranchesManagementService _branchesManagementService
) : ControllerBase  
{
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Branches")]
    public async Task<ActionResult<BranchDto>> GetById([FromQuery] Guid id)
    {
        Branch branch = await _branchesQueryService.GetByIdThrowsNotFoundAsync(id);

        return Ok(branch.ToDto());
    }

    [HttpGet("get-by-name")]
    [PermissionAuthorize("View", "Branches")]
    public async Task<ActionResult<BranchDto>> GetByName([FromQuery] string name)
    {
        Branch branch = await _branchesQueryService.GetByNameThrowsNotFoundAsync(name);

        return Ok(branch.ToDto());
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Branches")]
    public async Task<ActionResult<GetAllResponseDto<Branch>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Branch));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Branch));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(Branch));

        GetAllResponseDto<Branch> getAllResponseDto = await _branchesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Branches")]
    public async Task<ActionResult<BranchDto>> Create([FromBody] CreateBranchDto createBranchDto)
    {
        Branch branch = await _branchesManagementService.Create(createBranchDto);

        return Ok(branch.ToDto());
    }
    
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Branches")]
    public async Task<ActionResult<BranchDto>> Update([FromBody] UpdateBranchDto updateBranchDto)
    {
        Branch branch = await _branchesManagementService.Update(updateBranchDto);

        return Ok(branch.ToDto());
    }
}