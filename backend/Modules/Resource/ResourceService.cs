using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class ResourceService(
    IGenericService<Resource> _genericService
) : IResourceService
{
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

    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(r => r.ResourceId, id);
    }
}