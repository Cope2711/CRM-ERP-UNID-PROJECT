﻿using CRM_ERP_UNID.Data.Models;
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

    [HttpPut("update")]
    public async Task<ActionResult<RoleDto>> Update([FromBody] UpdateRoleDto updateRoleDto)
    {
        Role role = await _roleService.UpdateAsync(updateRoleDto);
        return Ok(Mapper.RoleToRoleDto(role));
    }

    [HttpGet("get-by-id")]
    public async Task<ActionResult<RoleDto>> GetById([FromQuery] Guid id)
    {
        Role role = await _roleService.GetByIdThrowsNotFoundAsync(id);

        RoleDto roleDto = new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
        };

        return Ok(roleDto);
    }

    [HttpGet("get-by-name")]
    public async Task<ActionResult<RoleDto>> GetByName([FromQuery] string roleName)
    {
        Role role = await _roleService.GetByNameThrowsNotFoundAsync(roleName);

        RoleDto roleDto = new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
        };

        return Ok(roleDto);
    }
    
    [HttpPost("get-all")]
    public async Task<ActionResult<GetAllResponseDto<RoleDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(Role));

        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(Role));

        GetAllResponseDto<Role> getAllResponseDto = await _roleService.GetAllAsync(getAllDto);
        GetAllResponseDto<RoleDto> getAllResponseDtoDto = new GetAllResponseDto<RoleDto>
        {
            Data = getAllResponseDto.Data.Select(Mapper.RoleToRoleDto).ToList(),
            TotalItems = getAllResponseDto.TotalItems,
            PageNumber = getAllResponseDto.PageNumber,
            PageSize = getAllResponseDto.PageSize,
            TotalPages = getAllResponseDto.TotalPages
        };
        
        return Ok(getAllResponseDtoDto);
    }

    [HttpPost("create")]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        Role newRole = await _roleService.CreateRoleAsync(createRoleDto);
        return CreatedAtAction(nameof(GetAll), new { id = newRole.RoleId }, newRole);
    }
    
    [HttpDelete("delete-by-id")]
    public async Task<ActionResult<RoleDto>> DeleteById([FromQuery] Guid id)
    {
        Role role = await _roleService.DeleteById(id);
        return Ok(Mapper.RoleToRoleDto(role));
    }
}