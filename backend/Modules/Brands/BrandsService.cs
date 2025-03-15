using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules.Brands;

public class BrandsService(
    IGenericService<Brand> _genericService,
    IBrandsRepository _brandsRepository,
    ILogger<BrandsService> _logger,
    IHttpContextAccessor _httpContextAccessor
) : IBrandsService
{
    public async Task<Brand> GetByIdThrowsNotFound(Guid brandId)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(brandId);
    }
    
    public async Task<Brand> GetByNameThrowsNotFound(string brandName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(b => b.BrandName, brandName);
    }
    
    public async Task<GetAllResponseDto<Brand>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }
    
    public async Task<bool> ExistByIdThrowsNotFound(Guid brandId)
    {
        if (!await _genericService.ExistsAsync(b => b.BrandId, brandId))
        {
            throw new NotFoundException(
                message: $"Brand with id: {brandId} not found!",
                field: Fields.Brands.BrandId);
        }
        return true;
    }
    
    public async Task<Brand> Create(CreateBrandDto createBrandDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for BrandName {TargetBrandName}",
            authenticatedUserId, createBrandDto.BrandName);
        
        // Check unique camps
        if (await ExistByName(createBrandDto.BrandName)){
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateAsync for BrandName {TargetBrandName} but the brandname already exists",
                authenticatedUserId, createBrandDto.BrandName);
            throw new UniqueConstraintViolationException("Brand with this name already exists", Fields.Brands.BrandName);
        }

        // Create brand
        Brand brand = new()
        {
            BrandName = createBrandDto.BrandName,
            BrandDescription = createBrandDto.BrandDescription,
            IsActive = createBrandDto.IsActive ?? true
        };

        _brandsRepository.Add(brand);

        await _brandsRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for BrandName {TargetBrandName} and the brand was created",
            authenticatedUserId, createBrandDto.BrandName);
        
        return brand;
    }
    
    public async Task<bool> ExistByName(string brandName)
    {
        return await _genericService.ExistsAsync(b => b.BrandName, brandName);
    }
    
    public async Task<Brand> Update(UpdateBrandDto updateBrandDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Brand brand = await GetByIdThrowsNotFound(updateBrandDto.BrandId);
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateAsync for BrandId {TargetBrandId}",
            authenticatedUserId, updateBrandDto.BrandId);
        
        bool hasChanges = ModelsHelper.UpdateModel(brand, updateBrandDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateBrandDto.BrandName):
                    return await ExistByName((string)value);
                
                default:
                    return false;
            }
        });
        
        if (hasChanges)
        {
            await _brandsRepository.SaveChangesAsync();
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for BrandId {TargetBrandId} and the brand was updated",
                authenticatedUserId, updateBrandDto.BrandId);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for BrandId {TargetBrandId} and the brand was not updated",
                authenticatedUserId, updateBrandDto.BrandId);
        }
        
        return brand;
    }
}