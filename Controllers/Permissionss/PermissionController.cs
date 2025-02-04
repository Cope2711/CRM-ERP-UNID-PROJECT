using Microsoft.AspNetCore.Mvc;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Controllers;

[ApiController]
[Route("api/permissions")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermissionDto>>> GetPermissions()
    {
        var permissions = await _permissionService.GetAllPermissionsAsync();
        return Ok(permissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PermissionDto>> GetPermissionById(Guid id)
    {
        var permission = await _permissionService.GetPermissionByIdAsync(id);
        return Ok(permission);
    }

    [HttpPost]
    public async Task<ActionResult<PermissionDto>> CreatePermission([FromBody] PermissionDto permissionDto)
    {
        var createdPermission = await _permissionService.CreatePermissionAsync(permissionDto);
        return CreatedAtAction(nameof(GetPermissionById), new { id = createdPermission.PermissionId }, createdPermission);
    }
}