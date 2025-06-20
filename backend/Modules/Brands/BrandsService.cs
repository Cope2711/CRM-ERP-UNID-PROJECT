using System.Text.Json;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class BrandsService(
    IGenericService<Brand> _genericService,
    IBrandsRepository _brandsRepository,
    ILogger<BrandsService> _logger,
    IHttpContextAccessor _httpContextAccessor
) : IBrandsService
{
    public async Task<Brand?> GetById(Guid id)
    {
        return await _genericService.GetById(id);
    }
    
    public async Task<Brand> GetByIdThrowsNotFound(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFound(id);
    }

    public async Task<Brand> GetByNameThrowsNotFound(string brandName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(b => b.name, brandName);
    }

    public async Task<GetAllResponseDto<Brand>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<bool> ExistByIdThrowsNotFound(Guid brandId)
    {
        if (!await _genericService.ExistsAsync(b => b.id == brandId))
        {
            throw new NotFoundException(
                message: $"Brand with id: {brandId} not found!",
                field: Fields.Brands.id);
        }

        return true;
    }

    public async Task<Brand> Create(Brand data)
    {
        return await _genericService.Create(data);
    }

    public async Task<bool> ExistByName(string brandName)
    {
        return await _genericService.ExistsAsync(b => b.name == brandName);
    }

    public async Task<Brand> Update(Guid id, JsonElement data)
    {
        Brand brand = await GetByIdThrowsNotFound(id);
        await _genericService.Update(brand, data);

        return brand;
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Brand? brand = await GetById(id);
            if (brand == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Brands.id, "Brand not found");
                continue;
            }

            if (!brand.isActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Brands.id,
                    "Brand was already deactivated");
                continue;
            }

            brand.isActive = false;
            await _brandsRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Brand successfully deactivated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Deactivate Brands request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Brand? brand = await GetById(id);
            if (brand == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Brands.id, "Brand not found");
                continue;
            }

            if (brand.isActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Brands.id,
                    "Brand was already activated");
                continue;
            }

            brand.isActive = true;
            await _brandsRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Brand successfully activated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Activate Brands request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }
}