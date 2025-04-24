using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class SalesQueryService(
    IGenericService<Sale> _genericService
    ) : ISalesQueryService
{
    public async Task<GetAllResponseDto<Sale>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }
    
    public async Task<Sale> GetByIdThrowsNotFound(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFound(id, query => query.Include(s => s.SaleDetails).ThenInclude(sd => sd.Product));
    }

}