using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
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
    public IActionResult GetSchema([FromQuery] string type)
    {
        if (!Utils.ValidSchemaTypes.Contains(type.ToLower()))
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");
        
        var dtoType = type.ToLower() switch
        {
            "create" => typeof(CreateRoleDto),
            "update" => typeof(UpdateRoleDto),
            "model" or "read" => typeof(RoleDto),
            _ => null
        };

        if (dtoType == null)
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");

        return Ok(DtoSchemaHelper.GetDtoSchema(dtoType));
    }
    
    [HttpPatch("update/{id}")]
    [PermissionAuthorize("Edit_Content", "Roles")]
    public async Task<ActionResult<RoleDto>> Update(Guid id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        Role role = await _rolesManagementService.Update(id, updateRoleDto);
        return Ok(role.ToDto());
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
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Role));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Role));

        GetAllResponseDto<Role> getAllResponseDto = await _rolesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }

    [HttpPost("create")]
    [PermissionAuthorize("Create", "Roles")]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        Role newRole = await _rolesManagementService.Create(createRoleDto);
        return CreatedAtAction(nameof(GetAll), new { id = newRole.RoleId }, newRole);
    }

    [HttpDelete("delete-by-id")]
    [PermissionAuthorize("Delete", "Roles")]
    public async Task<ActionResult<RoleDto>> DeleteById([FromQuery] Guid id)
    {
        Role role = await _rolesManagementService.DeleteById(id);
        return Ok(role.ToDto());
    }
}