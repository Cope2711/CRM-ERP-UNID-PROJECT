using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;


public interface IPermissionService
{
    Task<GetAllResponseDto<Permission>> GetAllAsync(GetAllDto getAllDto);
    Task<Permission> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Permission?> GetByNameAsync(string permissionName);
    Task<bool> ExistById(Guid id);
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

    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(p => p.PermissionId, id);
    }
}