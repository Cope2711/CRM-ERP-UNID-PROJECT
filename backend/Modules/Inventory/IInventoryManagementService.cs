using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IInventoryManagementService
{
    Task<Inventory> Create(CreateInventoryDto createInventoryDto);
    Task<Inventory> Update(UpdateInventoryDto updateInventoryDto);
}