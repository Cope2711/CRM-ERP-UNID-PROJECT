using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Controllers;


public interface IPermissionService
{
    Task<GetAllResponseDto<Permission>> GetAllAsync(GetAllDto getAllDto);
    Task<Permission> GetByIdThrowsNotFoundAsync(Guid id);
    Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto);
    Task<Permission?> GetByNameAsync(string permissionName);
}

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IGenericServie<Permission> _genericService;
    
    public PermissionService(IPermissionRepository permissionRepository, IGenericServie<Permission> genericService)
    {
        _permissionRepository = permissionRepository;
        _genericService = genericService;
    }

    public async Task<GetAllResponseDto<Permission>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<Permission> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id);
    }
    
    public async Task<Permission?> GetByNameAsync(string permissionName)
    {
        return await _genericService.GetFirstAsync(p => p.PermissionName, permissionName);
    }

    public async Task<PermissionDto> CreatePermissionAsync(PermissionDto permissionDto)
    {
        // Verificar si ya existe un permiso con el mismo nombre
        var existingPermission = await GetByNameAsync(permissionDto.PermissionName);
        if (existingPermission != null)
        {
            throw new UniqueConstraintViolationException($"A permission with the name '{permissionDto.PermissionName}' already exists.", field: "PermissionName");
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