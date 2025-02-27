using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IResourceService
{
    Task<GetAllResponseDto<Resource>> GetAllAsync(GetAllDto getAllDto);
    
    /// <summary>
    /// Retrieves all resources based on the specified filtering and pagination parameters.
    /// </summary>
    /// <param name="getAllDto">The DTO containing filtering and pagination parameters.</param>
    /// <returns>A response DTO containing the list of retrieved resources along with associated metadata.</returns>
    Task<Resource> GetByIdThrowsNotFoundAsync(Guid id);
    
    /// <summary>
    /// Retrieves a resource by its unique identifier and throws an exception if the resource is not found.
    /// </summary>
    /// <param name="id">The unique identifier of the resource.</param>
    /// <returns>The resource object with the specified identifier.</returns>
    /// <exception cref="NotFoundException">Thrown when no resource with the given identifier is found.</exception>
    Task<Resource> GetByNameThrowsNotFoundAsync(string resourceName);
    
    /// <summary>
    /// Retrieves a resource by its name and throws an exception if the resource is not found.
    /// </summary>
    /// <param name="resourceName">The name of the resource.</param>
    /// <returns>The resource object with the specified name.</returns>
    /// <exception cref="NotFoundException">Thrown when no resource with the specified name is found.</exception>
}
    
public class ResourceService : IResourceService
{
    private readonly IGenericServie<Resource> _genericService;
    
    public ResourceService(IGenericServie<Resource> genericService)
    {
        _genericService = genericService;
    }

    public async Task<GetAllResponseDto<Resource>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<Resource> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id);
    }
    
    public async Task<Resource> GetByNameThrowsNotFoundAsync(string resourceName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(r => r.ResourceName, resourceName);
    }
}
