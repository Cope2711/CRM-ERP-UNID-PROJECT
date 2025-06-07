using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
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
        return Ok(resource.ToDto());
    }

    [HttpGet("get-by-resourcename")]
    [PermissionAuthorize("View", "Resources")]
    public async Task<ActionResult<ResourceDto>> GetByName([FromQuery] string resourcename)
    {
        Resource resource = await _resourceService.GetByNameThrowsNotFoundAsync(resourcename);
        return Ok(resource.ToDto());
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Resources")]
    public async Task<ActionResult<GetAllResponseDto<Resource>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        GetAllResponseDto<Resource> getAllResponseDto = await _resourceService.GetAllAsync(getAllDto);

        return Ok(getAllResponseDto);
    }
}