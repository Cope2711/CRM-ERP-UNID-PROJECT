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
    public async Task<ActionResult<GetAllResponseDto<PermissionDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(Permission));
        
        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(Permission));
        GetAllResponseDto<Permission> getAllResponseDto = await _permissionService.GetAllAsync(getAllDto);
        return Ok(getAllResponseDto);
    }

    [HttpGet("get-by-id")]
    public async Task<ActionResult<PermissionDto>> GetById(Guid id)
    {
        Permission permission = await _permissionService.GetByIdThrowsNotFoundAsync(id);
        return Ok(permission);
    }

    [HttpPost]
    public async Task<ActionResult<PermissionDto>> CreatePermission([FromBody] PermissionDto permissionDto)
    {
        var createdPermission = await _permissionService.CreatePermissionAsync(permissionDto);
        return CreatedAtAction(nameof(GetById), new { id = createdPermission.PermissionId }, createdPermission);
    }
}