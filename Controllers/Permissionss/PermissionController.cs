using CRM_ERP_UNID.Data.Models;
using Microsoft.AspNetCore.Mvc;



namespace CRM_ERP_UNID.Controllers.Permissionss
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Permission>>> GetPermissions()
        {
            return Ok(await _permissionRepository.GetAllPermissionsAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Permission>> CreatePermission([FromBody] Permission permission)
        {
            var newPermission = await _permissionRepository.CreatePermissionAsync(permission);
            return CreatedAtAction(nameof(GetPermissions), new { id = newPermission.PermissionId }, newPermission);
        }
    }
}
