using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISalesQueryService
{
    Task<Sale> GetByIdThrowsNotFound(Guid id);
    Task<GetAllResponseDto<Sale>> GetAll(GetAllDto getAllDto);
}