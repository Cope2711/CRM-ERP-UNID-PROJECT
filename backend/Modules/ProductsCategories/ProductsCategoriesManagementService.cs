using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using CRM_ERP_UNID.Modules;

namespace CRM_ERP_UNID.Modules;

public class ProductsCategoriesManagementService(
    IProductsCategoriesQueryService _productsCategoriesQueryService,
    IProductsCategoriesRepository _productsCategoriesRepository,
    ILogger<ProductsCategoriesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IProductsQueryService _productsQueryService,
    ICategoriesQueryService _categoriesQueryService
) : IProductsCategoriesManagementService
{
    public async Task<ResponsesDto<ProductAndCategoryResponseStatusDto>> Assign(ProductsAndCategoriesDto productsAndCategoriesDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<ProductAndCategoryResponseStatusDto> responsesDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested Assign with the object {ProductsAndCategoriesDto}",
            authenticatedUserId, productsAndCategoriesDto);
        
        foreach (ProductAndCategoryIdDto productAndCategoryIdDto in productsAndCategoriesDto.ProductAndCategoryIdDto)
        {
            if (await _productsCategoriesQueryService.ExistsByProductCategoryIds(productAndCategoryIdDto.ProductId, productAndCategoryIdDto.CategoryId))
            {
                AddFailedResponse(responsesDto, productAndCategoryIdDto, ResponseStatus.AlreadyProcessed,
                    Fields.ProductsCategories.ProductCategoryId, "Product already assigned to category");
                continue;
            }
            
            if (!await _productsQueryService.ExistById(productAndCategoryIdDto.ProductId))
            {
                AddFailedResponse(responsesDto, productAndCategoryIdDto, ResponseStatus.NotFound,
                    Fields.Products.ProductId, "Product not exist");
                continue;
            }

            if (!await _categoriesQueryService.ExistsById(productAndCategoryIdDto.CategoryId))
            {
                AddFailedResponse(responsesDto, productAndCategoryIdDto, ResponseStatus.NotFound,
                    Fields.Categories.CategoryId, "Category not exist");
                continue;
            }

            ProductCategory productCategory = new()
            {
                ProductId = productAndCategoryIdDto.ProductId,
                CategoryId = productAndCategoryIdDto.CategoryId,
                CreatedDate = DateTime.UtcNow
            };
            
            _productsCategoriesRepository.Add(productCategory);
            await _productsCategoriesRepository.SaveChanges();
            
            responsesDto.Success.Add(new ProductAndCategoryResponseStatusDto
            {
                ProductAndCategoryId = productAndCategoryIdDto,
                Status = ResponseStatus.Success,
                Message = "ProductAssigned"
            });
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested Assign and the result was: {responsesDto}",
            authenticatedUserId, responsesDto);
        
        return responsesDto;
    }
    
    public async Task<ResponsesDto<IdResponseStatusDto>> Revoke(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responsesDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested Unassign with the object {IdsDto}",
            authenticatedUserId, idsDto);
        
        foreach (Guid productCategoryId in idsDto.Ids)
        {
            ProductCategory? productCategory = await _productsCategoriesQueryService.GetById(productCategoryId);
            
            if (productCategory == null)
            {
                ResponsesHelper.AddFailedResponseDto(responsesDto, productCategoryId, ResponseStatus.NotFound,
                    Fields.ProductsCategories.ProductCategoryId, "ProductCategory not exist");
                continue;
            }
            
            _productsCategoriesRepository.Remove(productCategory);
            await _productsCategoriesRepository.SaveChanges();
            
            responsesDto.Success.Add(new IdResponseStatusDto
            {
                Id = productCategoryId,
                Status = ResponseStatus.Success,
                Message = "ProductRevoked"
            });
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested Unassign and the result was: {responsesDto}",
            authenticatedUserId, responsesDto);
        
        return responsesDto;
    }
    
    private void AddFailedResponse(ResponsesDto<ProductAndCategoryResponseStatusDto> responsesDto, ProductAndCategoryIdDto productAndCategoryIdDto, string status, string field, string message)
    {
        responsesDto.Failed.Add(new ProductAndCategoryResponseStatusDto
        {
            ProductAndCategoryId = productAndCategoryIdDto,
            Status = status,
            Field = field,
            Message = message
        });
    }
}