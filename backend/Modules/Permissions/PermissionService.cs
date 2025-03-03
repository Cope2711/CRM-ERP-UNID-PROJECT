using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;


public interface IPermissionService
{
    Task<GetAllResponseDto<Permission>> GetAllAsync(GetAllDto getAllDto);
    Task<Permission> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Permission?> GetByNameAsync(string permissionName);
    Task<bool> ExistById(Guid id);
}

public class PermissionService(
    IPermissionRepository _permissionRepository,
    IGenericServie<Permission> _genericService
) : IPermissionService
{
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