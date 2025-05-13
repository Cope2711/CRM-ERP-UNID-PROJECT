using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class InventoryManagementService(
    IInventoryRepository _inventoryRepository,
    IInventoryQueryService _inventoryQueryService,
    ILogger<InventoryManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IBranchesQueryService _branchesQueryService,
    IUsersBranchesQueryService _usersBranchesQueryService,
    IGenericService<Inventory> _genericService
    ) : IInventoryManagementService
{
    public async Task IncreaseStockBulk(List<StockChangeDto> stockChanges, Guid branchId)
    {
        foreach (StockChangeDto stockChangeDto in stockChanges)
        {
            Inventory inventory = await _inventoryQueryService.GetByProductIdInBranchIdThrowsNotFound(stockChangeDto.ProductId, branchId);
            inventory.Quantity += stockChangeDto.Quantity;
        }

        await _inventoryRepository.SaveChangesAsync();
    }
    
    public async Task DecreaseStockBulk(List<StockChangeDto> stockChanges, Guid branchId)
    {
        foreach (StockChangeDto stockChangeDto in stockChanges)
        {
            Inventory inventory = await _inventoryQueryService.GetByProductIdInBranchIdThrowsNotFound(stockChangeDto.ProductId, branchId);

            if (inventory.Quantity - stockChangeDto.Quantity < 0)
            {
                throw new BadRequestException(
                    $"Not enough stock for product {stockChangeDto.ProductId}. Requested: {stockChangeDto.Quantity}, Available: {inventory.Quantity}",
                    field: Fields.Products.ProductId
                );
            }

            inventory.Quantity -= stockChangeDto.Quantity;
        }

        await _inventoryRepository.SaveChangesAsync();
    }
    
    public async Task<Inventory> Update(Guid id, UpdateInventoryDto updateInventoryDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        Inventory inventory = await _inventoryQueryService.GetByIdThrowsNotFoundAsync(id);

        if (updateInventoryDto.BranchId.HasValue && updateInventoryDto.BranchId != inventory.BranchId) 
        {
            await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId,
                updateInventoryDto.BranchId.Value);
        }

        if (updateInventoryDto.ProductId != null && 
            updateInventoryDto.ProductId != inventory.ProductId &&
            await _inventoryQueryService.ExistProductInBranch(inventory.ProductId, inventory.BranchId))
        {
            throw new UniqueConstraintViolationException(message: "This product already exists in this branch", field: Fields.Products.ProductId);
        }

        if (updateInventoryDto.BranchId != null &&
            updateInventoryDto.BranchId != inventory.BranchId &&
            await _inventoryQueryService.ExistProductInBranch(inventory.ProductId, inventory.BranchId))
        {
            throw new UniqueConstraintViolationException(message: "This branch already has this product", field: Fields.Branches.BranchId);
        }

        await _genericService.Update(inventory, updateInventoryDto); 

        return inventory;
    }
    
    public async Task<Inventory> Create(CreateInventoryDto createInventoryDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        // Exist branch?, user has access to branch?, product exists in branch?
        await _branchesQueryService.ExistsByIdThrowsNotFound(createInventoryDto.BranchId);
        await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId, createInventoryDto.BranchId);
        if (await _inventoryQueryService.ExistProductInBranch(createInventoryDto.ProductId, createInventoryDto.BranchId))
            throw new UniqueConstraintViolationException("This product already exists in the inventory", Fields.InventoryFields.ProductId);
        
        return await _genericService.Create(createInventoryDto.ToModel());
    }
}