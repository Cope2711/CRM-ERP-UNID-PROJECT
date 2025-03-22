using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IExampleQueryService
{
    Task<Example> GetByIdThrowsNotFoundAsync(Guid id);
    Task<GetAllResponseDto<Example>> GetAll(GetAllDto getAllDto);
    Task<bool> ExistByIdThrowsNotFound(Guid id);
    Task<bool> ExistById(Guid id);
    Task<bool> ExistByName(string exampleName);
}