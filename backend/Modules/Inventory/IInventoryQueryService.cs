using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IInventoryQueryService
{
    public Task<Inventory> GetByIdThrowsNotFoundAsync(Guid id);
    public Task<Inventory> GetByProductIdThrowsNotFoundAsync(Guid productId);
    public Task<GetAllResponseDto<Inventory>> GetAll(GetAllDto getAllDto);
    public Task<bool> ExistsByProductId(Guid productId);
}