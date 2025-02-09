using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IResourceService
{
    Task<GetAllResponseDto<Resource>> GetAllAsync(GetAllDto getAllDto);
    Task<Resource> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Resource> GetByNameThrowsNotFoundAsync(string resourceName);
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
