using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class ExampleQueryServices(IGenericService<Example> genericService) : IExampleQueryService
{
    public async Task<Example> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await genericService.GetByIdThrowsNotFoundAsync(id);
    }

    public async Task<GetAllResponseDto<Example>> GetAll(GetAllDto getAllDto)
    {
        return await genericService.GetAllAsync(getAllDto);
    }

    public async Task<bool> ExistByIdThrowsNotFound(Guid id)
    {
        return await genericService.ExistsAsync(e => e.ExampleId, id);
    }

    public async Task<bool> ExistById(Guid id)
    {
        return await genericService.ExistsAsync(e => e.ExampleId, id);
    }
    
    public async Task<bool> ExistByName(string exampleName)
    {
        return await genericService.ExistsAsync(e => e.ExampleName, exampleName);
    }
}