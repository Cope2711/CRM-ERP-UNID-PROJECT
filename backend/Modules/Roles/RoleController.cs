using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet("get-by-id")]
    public async Task<ActionResult<RoleDto>> GetRoleById([FromQuery] Guid id)
    {
        Role role = await this._roleService.GetByIdThrowsNotFoundAsync(id);
        
        RoleDto roleDto = new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            Permissions = role.RolePermissions?.Select(rp => new PermissionDto
            {
                PermissionId = rp.PermissionId,
                PermissionName = rp.Permission.PermissionName
            }).ToList()
        };
        
        return Ok(roleDto);
    }

    [HttpPost("get-all")]
    public async Task<ActionResult<List<Role>>> GetRoles([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(Role));
        
        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(Role));
        
        GetAllResponseDto<Role> getAllResponseDto = await _roleService.GetAllAsync(getAllDto);
        return Ok(getAllResponseDto);
    }

    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] RoleDto role)
    {
        var newRole = await _roleService.CreateRoleAsync(role);
        return CreatedAtAction(nameof(GetRoles), new { id = newRole.RoleId }, newRole);
    }

    [HttpPost("assign-permission")]
    public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionDto dto)
    {
        var role = await _roleService.AssignPermissionToRoleAsync(dto.RoleId, dto.PermissionId);
        return Ok(new { Message = "Permission assigned successfully", role });
    }
}