using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

//[ApiController]
[Authorize]
[Route("api/example")]
public class ExampleController(
    IExampleQueryService _exampleQueryService,
    IExampleManagementService _exampleManagementService
) : ControllerBase
{
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Example")]
    public async Task<ActionResult<ExampleDto>> GetById(Guid id)
    {
        Example example = await _exampleQueryService.GetByIdThrowsNotFoundAsync(id);
        return Ok(example.ToDto());
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Example")]
    public async Task<ActionResult<GetAllResponseDto<Example>>> GetAll(GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Example));
        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Example));
        
        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(Example));
        
        GetAllResponseDto<Example> getAllResponseDto = await _exampleQueryService.GetAll(getAllDto);
        
        return Ok(getAllResponseDto);
    }
    
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Example")]
    public async Task<ActionResult<ExampleDto>> UpdateExample([FromBody] UpdateExampleDto updateExampleDto)
    {
        Example example = await _exampleManagementService.Update(updateExampleDto);
        return Ok(example.ToDto());
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Example")]
    public async Task<ActionResult<ExampleDto>> CreateExample([FromBody] CreateExampleDto createExampleDto)
    {
        Example example = await _exampleManagementService.Create(createExampleDto);
        
        return Ok(example.ToDto());
    }
}