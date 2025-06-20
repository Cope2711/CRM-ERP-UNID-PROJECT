using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class InventoryQueryService(
    IGenericService<Inventory> _genericService,
    IInventoryRepository _inventoryRepository
    ) : IInventoryQueryService
{
    public async Task<Inventory> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFound(id, query => query.Include(i => i.Product));
    }
    
    public async Task<Inventory> GetByProductIdInBranchIdThrowsNotFound(Guid productId, Guid branchId)
    {
        Inventory? inventory = await _inventoryRepository.GetByProductIdInBranchId(productId, branchId);
        
        if (inventory == null)
            throw new NotFoundException("Product has no inventory", Fields.InventoryFields.productId);
        
        return inventory;
    }
    
    public async Task<GetAllResponseDto<Inventory>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }
    
    public async Task<bool> ExistProductInBranch(Guid productId, Guid branchId)
    {
        return await _inventoryRepository.ExistProductInBranch(productId, branchId);
    }
    
    public async Task<bool> ExistProductInBranchThrowsUniqueConstraintViolation(Guid productId, Guid branchId)
    {
        bool exists = await ExistProductInBranch(productId, branchId);

        if (!exists)
        {
            throw new UniqueConstraintViolationException("This product already exists in the inventory", Fields.InventoryFields.productId);   
        }
        
        return exists;
    }
}

