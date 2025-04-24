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
    IUsersBranchesQueryService _usersBranchesQueryService
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

        _logger.LogInformation(
            "User with id {UserId} is updating inventory with id {InventoryId} for product with id {ProductId} in branch {BranchId}",
            authenticatedUserId, inventory.InventoryId, inventory.ProductId, inventory.BranchId);

        bool hasChanges = ModelsHelper.UpdateModel(inventory, updateInventoryDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateInventoryDto.ProductId):
                    return await _inventoryQueryService.ExistProductInBranch((Guid)value, inventory.BranchId);

                case nameof(updateInventoryDto.BranchId):
                    return await _inventoryQueryService.ExistProductInBranch(inventory.ProductId, (Guid)value);

                default:
                    _logger.LogWarning("Unexpected field '{Field}' encountered during inventory update.", field);
                    return false;
            }
        });

        if (hasChanges)
        {
            await _inventoryRepository.SaveChangesAsync();
            _logger.LogInformation(
                "User with id {UserId} successfully updated inventory with id {InventoryId} for product with id {ProductId} in branch {BranchId}",
                authenticatedUserId, inventory.InventoryId, inventory.ProductId, inventory.BranchId);
        }
        else
        {
            _logger.LogInformation("No changes were made to inventory with id {InventoryId} by user with id {UserId}",
                inventory.InventoryId, authenticatedUserId);
        }

        return inventory;
    }


    public async Task<Inventory> Create(CreateInventoryDto createInventoryDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with id {UserId} is creating an inventory for product with id {ProductId}",
            authenticatedUserId, createInventoryDto.ProductId);

        // Exist branch?, user has access to branch?, product exists in branch?
        await _branchesQueryService.ExistsByIdThrowsNotFound(createInventoryDto.BranchId);
        await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId,
            createInventoryDto.BranchId);
        if (await _inventoryQueryService.ExistProductInBranch(createInventoryDto.ProductId,
                createInventoryDto.BranchId))
            throw new UniqueConstraintViolationException("This product already exists in the inventory",
                Fields.InventoryFields.ProductId);

        Inventory inventory = new()
        {
            ProductId = createInventoryDto.ProductId,
            BranchId = createInventoryDto.BranchId,
            Quantity = createInventoryDto.Quantity,
            IsActive = createInventoryDto.IsActive ?? true
        };

        _inventoryRepository.Add(inventory);
        await _inventoryRepository.SaveChangesAsync();

        _logger.LogInformation("User with id {UserId} has created an inventory for product with id {ProductId}",
            authenticatedUserId, createInventoryDto.ProductId);

        return inventory;
    }
}