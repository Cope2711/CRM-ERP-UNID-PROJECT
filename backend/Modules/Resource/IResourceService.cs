using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IResourceService
{
    Task<GetAllResponseDto<Resource>> GetAllAsync(GetAllDto getAllDto);
    Task<Resource> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Resource> GetByNameThrowsNotFoundAsync(string resourceName);
    Task<bool> ExistById(Guid id);
}