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
    IHttpContextAccessor _httpContextAccessor
) : IProductsManagementService
{
    public async Task<Product> ChangeBrand(ChangeBrandProductDto changeBrandProductDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Product product = await _productsQueryService.GetByIdThrowsNotFound(changeBrandProductDto.ProductId);
        
        if (product.BrandId == changeBrandProductDto.BrandId)
            return product;
        
        await _brandsService.ExistByIdThrowsNotFound(changeBrandProductDto.BrandId);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ChangeBrandAsync for ProductId {TargetProductId}",
            authenticatedUserId, changeBrandProductDto.ProductId);
        
        product.BrandId = changeBrandProductDto.BrandId;

        await _productsRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ChangeBrandAsync for ProductId {TargetProductId} and the product was changed",
            authenticatedUserId, changeBrandProductDto.ProductId);
        
        return product;
    }
    
    public async Task<Product> Create(CreateProductDto createProductDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested Create for ProductName {TargetProductName}",
            authenticatedUserId, createProductDto.ProductName);
        
        // Check unique camps
        if (await _productsQueryService.ExistByName(createProductDto.ProductName))
        {
            _logger.LogError(
                "User with Id {authenticatedUserId} requested Create for ProductName {TargetProductName} but the productname already exists",
                authenticatedUserId, createProductDto.ProductName);
            throw new UniqueConstraintViolationException("Product with this name already exists", Fields.Products.ProductName);
        }

        if (await _productsQueryService.ExistByBarcode(createProductDto.ProductBarcode))
        {
            _logger.LogError(
                "User with Id {authenticatedUserId} requested Create for ProductBarcode {TargetProductBarcode} but the productbarcode already exists",
                authenticatedUserId, createProductDto.ProductBarcode);
            throw new UniqueConstraintViolationException("Product with this barcode already exists",
                Fields.Products.ProductBarcode);
        }

        // Create product
        Product product = new()
        {
            ProductName = createProductDto.ProductName,
            ProductBarcode = createProductDto.ProductBarcode,
            ProductPrice = createProductDto.ProductPrice,
            ProductDescription = createProductDto.ProductDescription,
            IsActive = createProductDto.IsActive,
            BrandId = createProductDto.BrandId
        };

        _productsRepository.Add(product);

        await _productsRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for ProductName {TargetProductName} and the product was created",
            authenticatedUserId, createProductDto.ProductName);
        
        return product;
    }
    
    public async Task<Product> Update(Guid id, UpdateProductDto updateProductDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Product product = await _productsQueryService.GetByIdThrowsNotFound(id);
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateAsync for ProductId {TargetProductId}",
            authenticatedUserId, id);
        
        bool hasChanges = ModelsHelper.UpdateModel(product, updateProductDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateProductDto.ProductName):
                    return await _productsQueryService.ExistByName((string)value);
                case nameof(updateProductDto.ProductBarcode):
                    return await _productsQueryService.ExistByBarcode((string)value);
                
                default:
                    return false;
            }
        });
        
        if (hasChanges)
        {
            await _productsRepository.SaveChangesAsync();
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for ProductId {TargetProductId} and the product was updated",
                authenticatedUserId, id);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for ProductId {TargetProductId} and the product was not updated",
                authenticatedUserId, id);
        }
        
        return product;
    }
}