using System.Text.Json;
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
            Inventory inventory = await _inventoryQueryService.GetByProductIdInBranchIdThrowsNotFound(stockChangeDto.productId, branchId);
            inventory.quantity += stockChangeDto.quantity;
        }

        await _inventoryRepository.SaveChangesAsync();
    }
    
    public async Task DecreaseStockBulk(List<StockChangeDto> stockChanges, Guid branchId)
    {
        foreach (StockChangeDto stockChangeDto in stockChanges)
        {
            Inventory inventory = await _inventoryQueryService.GetByProductIdInBranchIdThrowsNotFound(stockChangeDto.productId, branchId);

            if (inventory.quantity - stockChangeDto.quantity < 0)
            {
                throw new BadRequestException(
                    $"Not enough stock for product {stockChangeDto.productId}. Requested: {stockChangeDto.quantity}, Available: {inventory.quantity}",
                    field: Fields.Products.id
                );
            }

            inventory.quantity -= stockChangeDto.quantity;
        }

        await _inventoryRepository.SaveChangesAsync();
    }
    
    public async Task<Inventory> Update(Guid id, JsonElement data)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        Inventory inventory = await _inventoryQueryService.GetByIdThrowsNotFoundAsync(id);

        string branchIdKey = nameof(Inventory.branchId)[0].ToString().ToLower() + nameof(Inventory.branchId)[1..];
        if (data.TryGetProperty(branchIdKey, out var branchIdElement) &&
            branchIdElement.ValueKind != JsonValueKind.Null &&
            Guid.TryParse(branchIdElement.GetString(), out Guid branchId) &&
            branchId != inventory.branchId)
        {
            await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId, branchId);

            if (await _inventoryQueryService.ExistProductInBranch(inventory.productId, branchId))
            {
                throw new UniqueConstraintViolationException(
                    message: "This branch already has this product",
                    field: Fields.Branches.id);
            }
        }

        string productIdKey = nameof(Inventory.productId)[0].ToString().ToLower() + nameof(Inventory.productId)[1..]; 
        if (data.TryGetProperty(productIdKey, out var productIdElement) &&
            productIdElement.ValueKind != JsonValueKind.Null &&
            Guid.TryParse(productIdElement.GetString(), out Guid productId) &&
            productId != inventory.productId)
        {
            if (await _inventoryQueryService.ExistProductInBranch(productId, inventory.branchId))
            {
                throw new UniqueConstraintViolationException(
                    message: "This product already exists in this branch",
                    field: Fields.Products.id);
            }
        }

        await _genericService.Update(inventory, data);

        return inventory;
    }
    
    public async Task<Inventory> Create(Inventory data)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        // Exist branch?, user has access to branch?, product exists in branch?
        await _branchesQueryService.ExistsByIdThrowsNotFound(data.branchId);
        await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId, data.branchId);
        if (await _inventoryQueryService.ExistProductInBranch(data.productId, data.branchId))
            throw new UniqueConstraintViolationException("This product already exists in the inventory", Fields.InventoryFields.productId);
        
        return await _genericService.Create(data);
    }
}