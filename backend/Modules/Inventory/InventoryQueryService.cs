using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class InventoryQueryService(
    IGenericService<Inventory> _genericService
    ) : IInventoryQueryService
{
    public async Task<Inventory> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id, query => query.Include(i => i.Product));
    }
    
    public async Task<Inventory> GetByProductIdThrowsNotFoundAsync(Guid productId)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(i => i.ProductId, productId, query => query.Include(i => i.Product));
    }
    
    public async Task<GetAllResponseDto<Inventory>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto, query => query.Include(i => i.Product));
    }
    
    public async Task<bool> ExistsByProductId(Guid productId)
    {
        return await _genericService.ExistsAsync(i => i.ProductId, productId);
    }
}

