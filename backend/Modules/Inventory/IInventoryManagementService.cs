using System.Text.Json;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IInventoryManagementService
{
    Task<Inventory> Create(Inventory data);
    Task<Inventory> Update(Guid id, JsonElement data);
    Task DecreaseStockBulk(List<StockChangeDto> stockChanges, Guid branchId);
    Task IncreaseStockBulk(List<StockChangeDto> stockChanges, Guid branchId);
}