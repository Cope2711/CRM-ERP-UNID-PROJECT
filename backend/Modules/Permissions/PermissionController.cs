using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using Microsoft.AspNetCore.Mvc;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/permissions")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Permissions")]
    public async Task<ActionResult<GetAllResponseDto<PermissionDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(Permission));
        
        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(Permission));
        GetAllResponseDto<Permission> getAllResponseDto = await _permissionService.GetAllAsync(getAllDto);
        GetAllResponseDto<PermissionDto> getAllResponseDtoDto = new GetAllResponseDto<PermissionDto>
        {
            Data = getAllResponseDto.Data.Select(Mapper.PermissionToPermissionDto).ToList(),
            TotalItems = getAllResponseDto.TotalItems,
            PageNumber = getAllResponseDto.PageNumber,
            PageSize = getAllResponseDto.PageSize,
            TotalPages = getAllResponseDto.TotalPages
        };
        
        return Ok(getAllResponseDtoDto);
    }

    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Permissions")]
    public async Task<ActionResult<PermissionDto>> GetById(Guid id)
    {
        Permission permission = await _permissionService.GetByIdThrowsNotFoundAsync(id);
        return Ok(Mapper.PermissionToPermissionDto(permission));
    }
}