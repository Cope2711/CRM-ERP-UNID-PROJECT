using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Controllers;

    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
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