using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;


public interface IPermissionService
{
    Task<GetAllResponseDto<Permission>> GetAllAsync(GetAllDto getAllDto);
    
    /// <summary>
    /// Recibe the permissionId and returns the Permission object if exists
    /// </summary>
    /// <param name="id">PermissionId</param>
    /// <returns>Permission</returns>
    /// <exception cref="NotFoundException">If not exist a permission with the id.</exception>>
    Task<Permission> GetByIdThrowsNotFoundAsync(Guid id);
    
    /// <summary>
    /// Retrieves a permission by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the permission.</param>
    /// <returns>The permission object.</returns>
    /// <exception cref="NotFoundException">Thrown when no permission with the given ID exists.</exception>
    Task<Permission?> GetByNameAsync(string permissionName);
    
    /// <summary>
    /// Retrieves a permission by its name.
    /// </summary>
    /// <param name="permissionName">The name of the permission.</param>
    /// <returns>The permission object if found; otherwise, null.</returns>

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
}