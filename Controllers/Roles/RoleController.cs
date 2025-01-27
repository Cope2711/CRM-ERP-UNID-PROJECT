using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Controllers.Roles
{
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(await _roleService.GetAllRolesAsync());

        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole([FromBody] Role role)
        {
            var newRole = await _roleService.CreateRoleAsync(role);
            return CreatedAtAction(nameof(GetRoles), new { id = newRole.RoleId }, newRole);


        }
        
        [HttpPost("assign-permission")]
        public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionDtos dto)
        {
            try
            {
                var role = await _roleService.AssignPermissionToRoleAsync(dto.RoleId, dto.PermissionId);
                return Ok(new { Message = "Permission assigned successfully", role });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        /*[HttpGet("roles-with-permission/{permissionId}")]
        public async Task<IActionResult> GetRolesWithPermission(Guid permissionId)
        {
            var roles = await _roleService.GetRolesWithPermissionAsync(permissionId);

            if (roles == null || roles.Count == 0)
            {
                return NotFound(new { message = "No roles found with this permission." });
            }

            return Ok(roles);
        }*/

    }
}
