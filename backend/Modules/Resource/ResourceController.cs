using CRM_ERP_UNID.Attributes;
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
    [PermissionAuthorize("View", "Resources")]
    public async Task<ActionResult<ResourceDto>> GetById([FromQuery] Guid id)
    {
        Resource resource = await _resourceService.GetByIdThrowsNotFoundAsync(id);
        return Ok(Mapper.ResourceToResourceDto(resource));
    }

    [HttpGet("get-by-resourcename")]
    [PermissionAuthorize("View", "Resources")]
    public async Task<ActionResult<ResourceDto>> GetByName([FromQuery] string resourcename)
    {
        Resource resource = await _resourceService.GetByNameThrowsNotFoundAsync(resourcename);
        return Ok(Mapper.ResourceToResourceDto(resource));
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Resources")]
    public async Task<ActionResult<GetAllResponseDto<ResourceDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Resource));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Resource));

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