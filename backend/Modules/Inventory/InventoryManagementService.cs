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
    IHttpContextAccessor _httpContextAccessor
) : IInventoryManagementService
{
    public async Task<Inventory> Update(UpdateInventoryDto updateInventoryDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Inventory inventory = await _inventoryQueryService.GetByIdThrowsNotFoundAsync(updateInventoryDto.InventoryId);
        
        _logger.LogInformation("User with id {UserId} is updating an inventory for product with id {ProductId}", authenticatedUserId, updateInventoryDto.ProductId);
        
        bool hasChanges = ModelsHelper.UpdateModel(inventory, updateInventoryDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateInventoryDto.ProductId):
                    return await _inventoryQueryService.ExistsByProductId((Guid)value);

                default:
                    return false;
            }
        });

        if (hasChanges)
        {
            await _inventoryRepository.SaveChangesAsync();
            _logger.LogInformation("User with id {UserId} has updated an inventory for product with id {ProductId}", authenticatedUserId, updateInventoryDto.ProductId);
        }
        else
        {
            _logger.LogInformation("User with id {UserId} has not updated an inventory for product with id {ProductId}", authenticatedUserId, updateInventoryDto.ProductId);
        }
        
        return inventory;
    }
    
    public async Task<Inventory> Create(CreateInventoryDto createInventoryDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with id {UserId} is creating an inventory for product with id {ProductId}", authenticatedUserId, createInventoryDto.ProductId);
        
        // Exist product?
        if (await _inventoryQueryService.ExistsByProductId(createInventoryDto.ProductId))
        {
            _logger.LogError("Product with id {ProductId} already exists in the inventory", createInventoryDto.ProductId);
            throw new UniqueConstraintViolationException("This product already exists in the inventory", Fields.InventoryFields.ProductId);   
        }
        
        Inventory inventory = new()
        {
            ProductId = createInventoryDto.ProductId,
            Quantity = createInventoryDto.Quantity,
            IsActive = createInventoryDto.IsActive ?? true
        };
        
        _inventoryRepository.Add(inventory);
        await _inventoryRepository.SaveChangesAsync();
        
        _logger.LogInformation("User with id {UserId} has created an inventory for product with id {ProductId}", authenticatedUserId, createInventoryDto.ProductId);
        
        return inventory;
    }
}