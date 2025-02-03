
using CRM_ERP_UNID.Dtos;

using CRM_ERP_UNID.Data.Models;
namespace CRM_ERP_UNID.Controllers;


public interface IPermissionService
{
    Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
    Task<PermissionDto> GetPermissionByIdAsync(Guid id);
    Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto);
    
    
}

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    
    
    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }
    
    public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
    {
        var permissions = await _permissionRepository.GetAllPermissionsAsync();
        return permissions.Select(p => new PermissionDto
        {
            PermissionId = p.PermissionId,
            PermissionName = p.PermissionName,
            Description = p.Description
        });
    }

    public async Task<PermissionDto> GetPermissionByIdAsync(Guid id)
    {
        var permission = await _permissionRepository.GetPermissionByIdAsync(id);
        if (permission == null)
        {
            throw new KeyNotFoundException("Permission not found.");
        }

        return new PermissionDto
        {
            PermissionId = permission.PermissionId,
            PermissionName = permission.PermissionName,
            Description = permission.Description
        };
    }

    public async Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto)
    {
        var permission = new Permission
        {
            PermissionId = Guid.NewGuid(),
            PermissionName = permissionDto.PermissionName,
            Description = permissionDto.Description
        };

        await _permissionRepository.CreatePermissionAsync(permission);

        return new PermissionDto
        {
            PermissionId = permission.PermissionId,
            PermissionName = permission.PermissionName,
            Description = permission.Description
        };
    }
}