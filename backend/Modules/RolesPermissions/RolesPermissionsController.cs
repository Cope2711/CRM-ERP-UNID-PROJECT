using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Route("api/roles-permissions")]
[Authorize]
public class RolesPermissionsController : ControllerBase
{
    private readonly IRolesPermissionsService _rolesPermissionsService;

    public RolesPermissionsController(IRolesPermissionsService rolesPermissionsService)
    {
        _rolesPermissionsService = rolesPermissionsService;
    }

    [HttpPost("assign-permission")]
    public async Task<ActionResult<RolePermissionDto>> AssignPermissionToRole([FromBody] PermissionAndRoleDto permissionAndRoleDto)
    {
        RolePermission rolePermission = await _rolesPermissionsService.AssignPermissionToRoleAsync(permissionAndRoleDto);
        return Ok(Mapper.RolePermissionToRolePermissionDto(rolePermission));
    }
    
    [HttpDelete("revoke-permission")]
    public async Task<ActionResult<RolePermissionDto>> RevokePermissionToRole([FromBody] PermissionAndRoleDto permissionAndRoleDto)
    {
        RolePermission rolePermission = await _rolesPermissionsService.RevokePermissionToRoleAsync(permissionAndRoleDto);
        return Ok(Mapper.RolePermissionToRolePermissionDto(rolePermission));
    }
    
    [HttpPost("get-all")]
    public async Task<ActionResult<GetAllResponseDto<RolePermissionDto>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.OrderBy, typeof(RolePermission));
        
        if (getAllDto.SearchColumn != null)
            CustomValidators.ValidateModelContainsColumnNameThrowsBadRequest(getAllDto.SearchColumn, typeof(RolePermission));
        
        GetAllResponseDto<RolePermission> getAllResponseDto = await _rolesPermissionsService.GetAllAsync(getAllDto);
        
        GetAllResponseDto<RolePermissionDto> getAllResponseDtoDto = new GetAllResponseDto<RolePermissionDto>();
        getAllResponseDtoDto.TotalItems = getAllResponseDto.TotalItems;
        getAllResponseDtoDto.TotalPages = getAllResponseDto.TotalPages;
        getAllResponseDtoDto.PageNumber = getAllResponseDto.PageNumber;
        getAllResponseDtoDto.PageSize = getAllResponseDto.PageSize;
        getAllResponseDtoDto.Data = getAllResponseDto.Data.Select(rp => Mapper.RolePermissionToRolePermissionDto(rp)).ToList();
        
        return Ok(getAllResponseDtoDto);
    }
}