using System.Text.Json;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/roles")]
public class RoleController(
    IRolesManagementService _rolesManagementService,
    IRolesQueryService _rolesQueryService
) : ControllerBase
{
    [HttpGet("schema")]
    [PermissionAuthorize("View", "Roles")]
    public IActionResult GetSchema([FromQuery] bool ignoreRequired = false)
    {
        return Ok(DtoSchemaHelper.GetDtoSchema(typeof(Role), ignoreRequired));
    }

    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Roles")]
    public async Task<ActionResult<RoleDto>> GetById([FromQuery] Guid id)
    {
        Role role = await _rolesQueryService.GetByIdThrowsNotFound(id);

        return Ok(role.ToDto());
    }

    [HttpGet("get-by-rolename")]
    [PermissionAuthorize("View", "Roles")]
    public async Task<ActionResult<RoleDto>> GetByName([FromQuery] string rolename)
    {
        Role role = await _rolesQueryService.GetByNameThrowsNotFound(rolename);

        return Ok(role.ToDto());
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Roles")]
    public async Task<ActionResult<GetAllResponseDto<Role>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        GetAllResponseDto<Role> getAllResponseDto = await _rolesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }

    [HttpPost("create")]
    [PermissionAuthorize("Create", "Roles")]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] Role data)
    {
        Role role = await _rolesManagementService.Create(data);
        return Ok(role);
    }
    
    [HttpPatch("update/{id}")]
    [PermissionAuthorize("Edit_Content", "Roles")]
    public async Task<ActionResult<RoleDto>> Update(Guid id, [FromBody] JsonElement data)
    {
        Role role = await _rolesManagementService.Update(id, data);
        return Ok(role.ToDto());
    }

    [HttpDelete("delete-by-id")]
    [PermissionAuthorize("Delete", "Roles")]
    public async Task<ActionResult<RoleDto>> DeleteById([FromQuery] Guid id)
    {
        Role role = await _rolesManagementService.DeleteById(id);
        return Ok(role.ToDto());
    }
}