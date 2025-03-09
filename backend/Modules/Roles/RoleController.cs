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
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Roles")]
    public async Task<ActionResult<RoleDto>> Update([FromBody] UpdateRoleDto updateRoleDto)
    {
        Role role = await _rolesManagementService.UpdateAsync(updateRoleDto);
        return Ok(role.ToDto());
    }

    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Roles")]
    public async Task<ActionResult<RoleDto>> GetById([FromQuery] Guid id)
    {
        Role role = await _rolesQueryService.GetByIdThrowsNotFoundAsync(id);

        return Ok(role.ToDto());
    }

    [HttpGet("get-by-rolename")]
    [PermissionAuthorize("View", "Roles")]
    public async Task<ActionResult<RoleDto>> GetByName([FromQuery] string rolename)
    {
        Role role = await _rolesQueryService.GetByNameThrowsNotFoundAsync(rolename);

        return Ok(role.ToDto());
    }

    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Roles")]
    public async Task<ActionResult<GetAllResponseDto<RoleDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Role));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Role));

        GetAllResponseDto<Role> getAllResponseDto = await _rolesQueryService.GetAllAsync(getAllDto);
        GetAllResponseDto<RoleDto> getAllResponseDtoDto = new GetAllResponseDto<RoleDto>
        {
            Data = getAllResponseDto.Data.Select(r => r.ToDto()).ToList(),
            TotalItems = getAllResponseDto.TotalItems,
            PageNumber = getAllResponseDto.PageNumber,
            PageSize = getAllResponseDto.PageSize,
            TotalPages = getAllResponseDto.TotalPages
        };

        return Ok(getAllResponseDtoDto);
    }

    [HttpPost("create")]
    [PermissionAuthorize("Create", "Roles")]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        Role newRole = await _rolesManagementService.CreateRoleAsync(createRoleDto);
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