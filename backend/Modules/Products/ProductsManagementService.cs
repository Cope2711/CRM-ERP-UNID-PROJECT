using System.Text.Json;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class ProductsManagementService(
    IProductsQueryService _productsQueryService,
    IProductsRepository _productsRepository,
    IBrandsService _brandsService,
    ILogger<ProductsManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IGenericService<Product> _genericService
    ) : IProductsManagementService
{
    public async Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Product? product = await _productsQueryService.GetById(id);
            if (product == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Products.id, "Product not found");
                continue;
            }

            if (!product.isActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Products.id,
                    "Product was already deactivated");
                continue;
            }

            product.isActive = false;
            await _productsRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Product successfully deactivated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Deactivate Products request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Product? product = await _productsQueryService.GetById(id);
            if (product == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Products.id, "Product not found");
                continue;
            }

            if (product.isActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Products.id,
                    "Product was already activated");
                continue;
            }

            product.isActive = true;
            await _productsRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Product successfully activated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Activate Products request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<Product> ChangeBrand(ChangeBrandProductDto changeBrandProductDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Product product = await _productsQueryService.GetByIdThrowsNotFound(changeBrandProductDto.ProductId);

        if (product.brandId == changeBrandProductDto.BrandId)
            return product;

        await _brandsService.ExistByIdThrowsNotFound(changeBrandProductDto.BrandId);

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ChangeBrandAsync for ProductId {TargetProductId}",
            authenticatedUserId, changeBrandProductDto.ProductId);

        product.brandId = changeBrandProductDto.BrandId;

        await _productsRepository.SaveChanges();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ChangeBrandAsync for ProductId {TargetProductId} and the product was changed",
            authenticatedUserId, changeBrandProductDto.ProductId);

        return product;
    }

    public async Task<Product> Create(Product data)
    {
        return await _genericService.Create(data);
    }

    public async Task<Product> Update(Guid id, JsonElement data)
    {
        Product product = await _productsQueryService.GetByIdThrowsNotFound(id);

        await _genericService.Update(product, data);

        return product;
    }
}