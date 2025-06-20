using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class SalesManagementService(
    ILogger<SalesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IProductsQueryService _productsQueryService,
    IBranchesQueryService _branchesQueryService,
    ISalesRepository _salesRepository,
    ISalesQueryService _salesQueryService,
    IInventoryManagementService _inventoryManagementService
    ) : ISalesManagementService
{
    public async Task Delete(Guid id)
    {
        // Inicia una transacción
        using var transaction = await _salesRepository.BeginTransactionAsync();
        try
        {
            // Obtén la venta que deseas deshacer
            Sale sale = await _salesQueryService.GetByIdThrowsNotFound(id);

            // Revertir el inventario (restaurar el stock de los productos vendidos)
            var stockChanges = sale.SaleDetails.Select(sd => new StockChangeDto
            {
                productId = sd.productId,
                quantity = sd.quantity
            }).ToList();
        
            await _inventoryManagementService.IncreaseStockBulk(stockChanges, sale.branchId);

            // Eliminar los detalles de la venta y la venta en sí
            _salesRepository.Remove(sale);

            // Guardar cambios y confirmar transacción
            await _salesRepository.SaveChanges();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            // Si hay algún error, revertir la transacción
            await transaction.RollbackAsync();
            throw; // Rethrow exception to be handled at a higher level
        }
    }
    
    public async Task<Sale> Create(CreateFullSaleDto createFullSaleDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with Id {authenticatedUserId} requested CreateSale", authenticatedUserId);

        // Start a transaction
        using (var transaction = await _salesRepository.BeginTransactionAsync())
        {
            try
            {
                // Check if Branch exists
                await _branchesQueryService.ExistsByIdThrowsNotFound(createFullSaleDto.branchId);

                // Check if all products exist
                foreach (CreateSaleDetailDto createSaleDetailDto in createFullSaleDto.SaleDetails)
                {
                    await _productsQueryService.ExistByIdThrowsNotFound(createSaleDetailDto.productId);
                }

                // Prepare the sale
                var sale = new Sale
                {
                    branchId = createFullSaleDto.branchId,
                    userId = authenticatedUserId,
                    saleDate = DateTime.UtcNow,
                    totalAmount = createFullSaleDto.totalAmount,
                    createdDate = DateTime.UtcNow,
                    updatedDate = DateTime.UtcNow,
                    SaleDetails = createFullSaleDto.SaleDetails.Select(detail => new SaleDetail
                    {
                        productId = detail.productId,
                        quantity = detail.quantity,
                        unitPrice = detail.unitPrice
                    }).ToList()
                };

                // Prepare stock change DTOs
                List<StockChangeDto> stockChanges = createFullSaleDto.SaleDetails.Select(detail => new StockChangeDto
                {
                    productId = detail.productId,
                    quantity = detail.quantity
                }).ToList();

                // Decrease stock
                await _inventoryManagementService.DecreaseStockBulk(stockChanges, createFullSaleDto.branchId);

                // Add sale to repository
                _salesRepository.Add(sale);
                await _salesRepository.SaveChanges();

                // Commit the transaction
                await transaction.CommitAsync();

                return sale;
            }
            catch (Exception ex)
            {
                // In case of error, rollback transaction
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating sale with details.");
                throw; // Rethrow the exception
            }
        }
    }
}