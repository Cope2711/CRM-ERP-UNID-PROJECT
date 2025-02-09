using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[Authorize]
[Route("api/resources")]
[ApiController]
public class ResourceController : ControllerBase
{
    private readonly IResourceService _resourceService;

    public ResourceController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpGet("get-by-id")]
    public async Task<ActionResult<ResourceDto>> GetById([FromQuery] Guid id)
    {
        Resource resource = await _resourceService.GetByIdThrowsNotFoundAsync(id);
        return Ok(Mapper.ResourceToResourceDto(resource));
    }

    [HttpGet("get-by-name")]
    public async Task<ActionResult<ResourceDto>> GetByName([FromQuery] string resourceName)
    {
        Resource resource = await _resourceService.GetByNameThrowsNotFoundAsync(resourceName);
        return Ok(Mapper.ResourceToResourceDto(resource));
    }

    [HttpPost("get-all")]
    public async Task<ActionResult<GetAllResponseDto<ResourceDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(Resource));

        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(Resource));

        GetAllResponseDto<Resource> getAllResponseDto = await _resourceService.GetAllAsync(getAllDto);
        GetAllResponseDto<ResourceDto> getAllResponseDtoDto = new GetAllResponseDto<ResourceDto>
        {
            Data = getAllResponseDto.Data.Select(Mapper.ResourceToResourceDto).ToList(),
            TotalItems = getAllResponseDto.TotalItems,
            PageNumber = getAllResponseDto.PageNumber,
            PageSize = getAllResponseDto.PageSize,
            TotalPages = getAllResponseDto.TotalPages
        };

        return Ok(getAllResponseDtoDto);
    }
}