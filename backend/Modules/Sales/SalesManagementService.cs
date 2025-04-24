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
                ProductId = sd.ProductId,
                Quantity = sd.Quantity
            }).ToList();
        
            await _inventoryManagementService.IncreaseStockBulk(stockChanges, sale.BranchId);

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
                await _branchesQueryService.ExistsByIdThrowsNotFound(createFullSaleDto.BranchId);

                // Check if all products exist
                foreach (CreateSaleDetailDto createSaleDetailDto in createFullSaleDto.SaleDetails)
                {
                    await _productsQueryService.ExistByIdThrowsNotFound(createSaleDetailDto.ProductId);
                }

                // Prepare the sale
                var sale = new Sale
                {
                    BranchId = createFullSaleDto.BranchId,
                    UserId = authenticatedUserId,
                    SaleDate = DateTime.UtcNow,
                    TotalAmount = createFullSaleDto.TotalAmount,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    SaleDetails = createFullSaleDto.SaleDetails.Select(detail => new SaleDetail
                    {
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice
                    }).ToList()
                };

                // Prepare stock change DTOs
                List<StockChangeDto> stockChanges = createFullSaleDto.SaleDetails.Select(detail => new StockChangeDto
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity
                }).ToList();

                // Decrease stock
                await _inventoryManagementService.DecreaseStockBulk(stockChanges, createFullSaleDto.BranchId);

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