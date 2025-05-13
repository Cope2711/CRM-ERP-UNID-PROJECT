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
    ISuppliersProductsQueryService _suppliersProductsQueryService,
    IGenericService<SupplierProduct> _genericService
) : ISuppliersProductsManagementService
{
    public async Task<SupplierProduct> Update(UpdateSupplierProductDto updateSupplierProductDto)
    {
        SupplierProduct? supplierProduct = await _suppliersProductsQueryService.GetByIdThrowsNotFound(updateSupplierProductDto.SupplierProductId);
        
        await _genericService.Update(supplierProduct, updateSupplierProductDto);
        
        return supplierProduct;
    }
    
    public async Task<ResponsesDto<ModelAndAssignResponseStatusDto>> AssignProductsToSuppliers(
        ModelsAndAssignsDtos modelAndAssignsDtos)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<ModelAndAssignResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignProductsToSuppliers with the object {SuppliersAndProductsIdsDto}",
            authenticatedUserId, modelAndAssignsDtos);

        foreach (ModelAssignIdsDto modelAssignIdsDto in modelAndAssignsDtos.ModelAssignIds)
        {
            if (await _suppliersProductsRepository.IsProductAssignedToSupplier(modelAssignIdsDto.ModelId,
                    modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.AlreadyProcessed,
                    Fields.Products.ProductId, "Product already assigned to supplier");
                continue;
            }

            if (!await _suppliersQueryService.ExistById(modelAssignIdsDto.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Suppliers.SupplierId, "Supplier not exist");
                continue;
            }

            if (!await _productsQueryService.ExistById(modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Products.ProductId, "Product not exist");
                continue;
            }

            // Add to database
            SupplierProduct supplierProduct = new()
            {
                SupplierId = modelAssignIdsDto.ModelId,
                ProductId = modelAssignIdsDto.AssignId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _suppliersProductsRepository.Add(supplierProduct);
            await _suppliersProductsRepository.SaveChangesAsync();

            responseDto.Success.Add(new ModelAndAssignResponseStatusDto
            {
                ModelAssignIds = modelAssignIdsDto,
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
}