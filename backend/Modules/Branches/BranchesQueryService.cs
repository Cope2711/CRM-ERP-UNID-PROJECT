using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public class BranchesQueryService(
    IGenericService<Branch> _genericService
) : IBranchesQueryService
{
    public Task<Branch> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return _genericService.GetByIdThrowsNotFoundAsync(id);
    }

    public async Task<Branch> GetByNameThrowsNotFoundAsync(string name)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(b => b.BranchName, name);
    }
    
    public async Task<GetAllResponseDto<Branch>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }
    
    public async Task<bool> ExistByName(string name)
    {
        return await _genericService.ExistsAsync(b => b.BranchName, name);
    }
}