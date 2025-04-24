using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISalesManagementService
{
    Task<Sale> Create(CreateFullSaleDto createFullSaleDto);
    Task Delete(Guid id);
}