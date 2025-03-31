using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class SuppliersProductsManagementService(
    ISuppliersProductsRepository _suppliersProductsRepository,
    ILogger<SuppliersProductsManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    ISuppliersQueryService _suppliersQueryService,
    IProductsQueryService _productsQueryService,
    ISuppliersProductsQueryService _suppliersProductsQueryService
) : ISuppliersProductsManagementService
{
    public async Task<SupplierProduct> Update(UpdateSupplierProductDto updateSupplierProductDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateSupplierProduct with SupplierProductId {TargetSupplierProductId}",
            authenticatedUserId, updateSupplierProductDto.SupplierProductId);
        
        SupplierProduct? supplierProduct = await _suppliersProductsQueryService.GetByIdThrowsNotFound(updateSupplierProductDto.SupplierProductId);
        
        bool hasChanges = ModelsHelper.UpdateModel(supplierProduct, updateSupplierProductDto, async (field, value) =>
        {
            return field switch
            {
                nameof(updateSupplierProductDto.SupplyPrice) => value == null,
                nameof(updateSupplierProductDto.SupplyLeadTime) => value == null,
                _ => false
            };
        });
        
        if (hasChanges)
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} updated SupplierProduct with Id {UpdatedSupplierProductId}",
                authenticatedUserId, supplierProduct.SupplierProductId);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateSupplierProduct but no changes were made",
                authenticatedUserId);
        }
        
        return supplierProduct;
    }
    
    public async Task<ResponsesDto<SupplierAndProductResponseStatusDto>> AssignProductsToSuppliers(
        SuppliersAndProductsIdsDto suppliersAndProductsIdsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<SupplierAndProductResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignProductsToSuppliers with the object {SuppliersAndProductsIdsDto}",
            authenticatedUserId, suppliersAndProductsIdsDto);

        foreach (SupplierAndProductIdDto supplierAndProductIdDto in suppliersAndProductsIdsDto.SuppliersAndProductsIds)
        {
            if (await _suppliersProductsRepository.IsProductAssignedToSupplier(supplierAndProductIdDto.SupplierId,
                    supplierAndProductIdDto.ProductId))
            {
                AddFailedResponseDto(responseDto, supplierAndProductIdDto, ResponseStatus.AlreadyProcessed,
                    Fields.Products.ProductId, "Product already assigned to supplier");
                continue;
            }

            if (!await _suppliersQueryService.ExistById(supplierAndProductIdDto.SupplierId))
            {
                AddFailedResponseDto(responseDto, supplierAndProductIdDto, ResponseStatus.NotFound,
                    Fields.Suppliers.SupplierId, "Supplier not exist");
                continue;
            }

            if (!await _productsQueryService.ExistById(supplierAndProductIdDto.ProductId))
            {
                AddFailedResponseDto(responseDto, supplierAndProductIdDto, ResponseStatus.NotFound,
                    Fields.Products.ProductId, "Product not exist");
                continue;
            }

            // Add to database
            SupplierProduct supplierProduct = new()
            {
                SupplierId = supplierAndProductIdDto.SupplierId,
                ProductId = supplierAndProductIdDto.ProductId,
                SupplyPrice = supplierAndProductIdDto.SupplyPrice,
                SupplyLeadTime = supplierAndProductIdDto.SupplyLeadTime,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _suppliersProductsRepository.Add(supplierProduct);
            await _suppliersProductsRepository.SaveChangesAsync();

            responseDto.Success.Add(new SupplierAndProductResponseStatusDto
            {
                SupplierAndProductId = supplierAndProductIdDto,
                Status = ResponseStatus.Success,
                Message = "ProductAssigned"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignProductsToSuppliers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> RevokeProductsFromSuppliers(
        IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UnassignProductsFromSuppliers with the object {SuppliersProductsIdsDto}",
            authenticatedUserId, idsDto);

        foreach (Guid supplierProductId in idsDto.Ids)
        {
            SupplierProduct? supplierProduct =
                await _suppliersProductsQueryService.GetById(supplierProductId);

            if (supplierProduct == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, supplierProductId, ResponseStatus.NotFound,
                    Fields.SuppliersProducts.SupplierProductId, "SupplierProduct not exist");
                continue;
            }
            
            _suppliersProductsRepository.Remove(supplierProduct);
            await _suppliersProductsRepository.SaveChangesAsync();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = supplierProductId,
                Status = ResponseStatus.Success,
                Message = "ProductRevoked"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UnassignProductsFromSuppliers and the result was: {responseDto}",
            authenticatedUserId, responseDto);
        return responseDto;
    }

    public void AddFailedResponseDto(ResponsesDto<SupplierAndProductResponseStatusDto> responseDto,
        SupplierAndProductIdDto supplierAndProductIdDto, string status, string field, string message)
    {
        responseDto.Failed.Add(new SupplierAndProductResponseStatusDto
        {
            SupplierAndProductId = supplierAndProductIdDto,
            Status = status,
            Field = field,
            Message = message
        });
    }
}