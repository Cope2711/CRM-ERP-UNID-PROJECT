using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;

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
    public async Task<ResponsesDto<ModelAndAssignResponseStatusDto>> Assign(ModelsAndAssignsDtos modelsAndAssignsDtos)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<ModelAndAssignResponseStatusDto> responsesDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested Assign with the object {ModelsAndAssignsDtos}",
            authenticatedUserId, modelsAndAssignsDtos);
        
        foreach (ModelAssignIdsDto modelAssignIdsDto in modelsAndAssignsDtos.ModelAssignIds)
        {
            if (await _productsCategoriesQueryService.ExistsByProductCategoryIds(modelAssignIdsDto.ModelId, modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responsesDto, modelAssignIdsDto, ResponseStatus.AlreadyProcessed,
                    Fields.ProductsCategories.ProductCategoryId, "Product already assigned to category");
                continue;
            }
            
            if (!await _productsQueryService.ExistById(modelAssignIdsDto.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responsesDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Products.ProductId, "Product not exist");
                continue;
            }

            if (!await _categoriesQueryService.ExistsById(modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responsesDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Categories.CategoryId, "Category not exist");
                continue;
            }

            ProductCategory productCategory = new()
            {
                ProductId = modelAssignIdsDto.ModelId,
                CategoryId = modelAssignIdsDto.AssignId,
                CreatedDate = DateTime.UtcNow
            };
            
            _productsCategoriesRepository.Add(productCategory);
            await _productsCategoriesRepository.SaveChanges();
            
            responsesDto.Success.Add(new ModelAndAssignResponseStatusDto
            {
                ModelAssignIds = modelAssignIdsDto,
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
}