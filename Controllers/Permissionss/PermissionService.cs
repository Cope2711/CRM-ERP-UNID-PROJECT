
using CRM_ERP_UNID.Dtos;

using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;

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
            throw new NotFoundException($"Permission with ID {id} not found.", field: "permissionId");
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
        // Verificar si ya existe un permiso con el mismo nombre
        var existingPermission = await _permissionRepository.GetByName(permissionDto.PermissionName);
        if (existingPermission != null)
        {
            throw new UniqueConstraintViolationException($"A permission with the name '{permissionDto.PermissionName}' already exists.", field: "permissionName");
        }

        var permission = new Permission
        {
            PermissionId = Guid.NewGuid(),
            PermissionName = permissionDto.PermissionName,
            Description = permissionDto.Description
        };

        this._permissionRepository.AddPermissionAsync(permission);
        await this._permissionRepository.SaveChangesAsync();

        return new PermissionDto
        {
            PermissionId = permission.PermissionId,
            PermissionName = permission.PermissionName,
            Description = permission.Description
        };
    }
}