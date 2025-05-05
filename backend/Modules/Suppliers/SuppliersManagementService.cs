using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class SuppliersManagementService(
    ISuppliersRepository _suppliersRepository,
    ISuppliersQueryService _suppliersQueryService,
    ILogger<SuppliersManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IGenericService<Supplier> _genericService
    ) : ISuppliersManagementService
{
    public async Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Supplier? supplier = await _suppliersQueryService.GetById(id);
            if (supplier == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Suppliers.SupplierId, "Supplier not found");
                continue;
            }

            if (!supplier.IsActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Suppliers.SupplierId,
                    "Supplier was already deactivated");
                continue;
            }

            supplier.IsActive = false;
            await _suppliersRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Supplier successfully deactivated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Deactivate Suppliers request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Supplier? supplier = await _suppliersQueryService.GetById(id);
            if (supplier == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Suppliers.SupplierId, "Supplier not found");
                continue;
            }

            if (supplier.IsActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Suppliers.SupplierId,
                    "Supplier was already activated");
                continue;
            }

            supplier.IsActive = true;
            await _suppliersRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Supplier successfully activated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Activate Suppliers request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }
    
    public async Task<Supplier> Create(CreateSupplierDto createSupplierDto)
    {
        return await _genericService.Create(createSupplierDto.ToModel());
    }

    public async Task<Supplier> Update(Guid id, UpdateSupplierDto updateSupplierDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Supplier supplier = await _suppliersQueryService.GetByIdThrowsNotFoundAsync(id);

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateAsync for SupplierId {TargetSupplierId}",
            authenticatedUserId, id);

        bool hasChanges = ModelsHelper.UpdateModel(supplier, updateSupplierDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateSupplierDto.SupplierName):
                    return await _suppliersQueryService.ExistByName((string)value);
                case nameof(updateSupplierDto.SupplierEmail):
                    return await _suppliersQueryService.ExistByEmail((string)value);

                default:
                    return false;
            }
        });

        if (hasChanges)
        {
            await _suppliersRepository.SaveChanges();
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for SupplierId {TargetSupplierId} and the supplier was updated",
                authenticatedUserId, id);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for SupplierId {TargetSupplierId} and the supplier was not updated",
                authenticatedUserId, id);
        }

        return supplier;
    }
}