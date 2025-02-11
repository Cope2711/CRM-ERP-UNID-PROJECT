using CRM_ERP_UNID.Attributes;
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

    [HttpPost("assign-permission")]
    [PermissionAuthorize("Assign_Permission")]
    public async Task<ActionResult<RolePermissionResourceDto>> AssignPermissionToRole([FromBody] PermissionResourceAndRoleDto permissionResourceAndRoleDto)
    {
        RolePermissionResource rolePermissionResource = await _rolesPermissionsResourcesService.AssignPermissionToRoleAsync(permissionResourceAndRoleDto);
        return Ok(Mapper.RolePermissionResourceToRolePermissionResourceDto(rolePermissionResource));
    }
    
    [HttpDelete("revoke-permission")]
    [PermissionAuthorize("Revoke_Permission")]
    public async Task<ActionResult<RolePermissionResourceDto>> RevokePermissionToRole([FromBody] PermissionResourceAndRoleDto permissionResourceAndRoleDto)
    {
        RolePermissionResource rolePermissionResource = await _rolesPermissionsResourcesService.RevokePermissionToRoleAsync(permissionResourceAndRoleDto);
        return Ok(Mapper.RolePermissionResourceToRolePermissionResourceDto(rolePermissionResource));
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "RolesPermissionsResources")]
    public async Task<ActionResult<GetAllResponseDto<RolePermissionResourceDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(RolePermissionResource));
        
        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(RolePermissionResource));
        
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