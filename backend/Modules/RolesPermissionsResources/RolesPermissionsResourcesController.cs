﻿using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Route("api/roles-permissions")]
[Authorize]
public class RolesPermissionsResourcesController : ControllerBase 
{
    private readonly IRolesPermissionsResourcesService _rolesPermissionsResourcesService;
    
    public RolesPermissionsResourcesController(IRolesPermissionsResourcesService rolesPermissionsResourcesService)
    {
        _rolesPermissionsResourcesService = rolesPermissionsResourcesService;
    }

    [HttpPost("assign-permissions")]
    [PermissionAuthorize("Assign_Permission")]
    public async Task<ActionResult<ResponsesDto<RolePermissionResourceResponseStatusDto>>> AssignPermissionsToRoles([FromBody] PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto)
    {
        ResponsesDto<RolePermissionResourceResponseStatusDto> responsesDto = await _rolesPermissionsResourcesService.AssignPermissionsToRolesAsync(permissionsResourcesAndRolesIdsDto);
        return Ok(responsesDto);
    }
    
    [HttpDelete("revoke-permissions")]
    [PermissionAuthorize("Revoke_Permission")]
    public async Task<ActionResult<ResponsesDto<RolePermissionResourceResponseStatusDto>>> RevokePermissionsToRoles([FromBody] PermissionsResourcesAndRolesIdsDto permissionsResourcesAndRolesIdsDto)
    {
        ResponsesDto<RolePermissionResourceResponseStatusDto> responsesDto = await _rolesPermissionsResourcesService.RevokePermissionsToRolesAsync(permissionsResourcesAndRolesIdsDto);
        return Ok(responsesDto);
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "RolesPermissionsResources")]
    public async Task<ActionResult<GetAllResponseDto<RolePermissionResourceDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(RolePermissionResource));
        
        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(RolePermissionResource));
        
        GetAllResponseDto<RolePermissionResource> getAllResponseDto = await _rolesPermissionsResourcesService.GetAllAsync(getAllDto);
        
        GetAllResponseDto<RolePermissionResourceDto> getAllResponseDtoDto = new GetAllResponseDto<RolePermissionResourceDto>();
        getAllResponseDtoDto.TotalItems = getAllResponseDto.TotalItems;
        getAllResponseDtoDto.TotalPages = getAllResponseDto.TotalPages;
        getAllResponseDtoDto.PageNumber = getAllResponseDto.PageNumber;
        getAllResponseDtoDto.PageSize = getAllResponseDto.PageSize;
        getAllResponseDtoDto.Data = getAllResponseDto.Data.Select(rp => Mapper.RolePermissionResourceToRolePermissionResourceDto(rp)).ToList();
        
        return Ok(getAllResponseDtoDto);
    }
}