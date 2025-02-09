using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Route("api/permissions-resources")]
[Authorize]
public class PermissionsResourcesController : ControllerBase
{
    private readonly IPermissionsResourcesService _permissionsResourcesService;

    public PermissionsResourcesController(IPermissionsResourcesService permissionsResourcesService)
    {
        _permissionsResourcesService = permissionsResourcesService;
    }
    
    [HttpPost("get-all")]
    public async Task<ActionResult<GetAllResponseDto<PermissionResourceDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(PermissionResource));
        
        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(PermissionResource));
        
        GetAllResponseDto<PermissionResource> getAllResponseDto = await _permissionsResourcesService.GetAllAsync(getAllDto);
        GetAllResponseDto<PermissionResourceDto> getAllResponseDtoDto = new GetAllResponseDto<PermissionResourceDto>
        {
            Data = getAllResponseDto.Data.Select(Mapper.PermissionResourceToPermissionResourceDto).ToList(),
            TotalItems = getAllResponseDto.TotalItems,
            PageNumber = getAllResponseDto.PageNumber,
            PageSize = getAllResponseDto.PageSize,
            TotalPages = getAllResponseDto.TotalPages
        };
        
        return Ok(getAllResponseDtoDto);
    }
}