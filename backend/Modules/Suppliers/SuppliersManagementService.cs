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
    IHttpContextAccessor _httpContextAccessor
) : ISuppliersManagementService
{
    public async Task<Supplier> Create(CreateSupplierDto createSupplierDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for SupplierName {TargetSupplierName}",
            authenticatedUserId, createSupplierDto.SupplierName);
        
        // Check unique camps
        if (await _suppliersQueryService.ExistByName(createSupplierDto.SupplierName)){
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateAsync for SupplierName {TargetSupplierName} but the suppliername already exists",
                authenticatedUserId, createSupplierDto.SupplierName);
            throw new UniqueConstraintViolationException("Supplier with this name already exists", Fields.Suppliers.SupplierName);
        }
        
        if (await _suppliersQueryService.ExistByEmail(createSupplierDto.SupplierEmail)){
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateAsync for SupplierEmail {TargetSupplierEmail} but the supplieremail already exists",
                authenticatedUserId, createSupplierDto.SupplierEmail);
            throw new UniqueConstraintViolationException("Supplier with this email already exists", Fields.Suppliers.SupplierEmail);
        }

        // Create supplier
        Supplier supplier = new()
        {
            SupplierName = createSupplierDto.SupplierName,
            SupplierContact = createSupplierDto.SupplierContact,
            SupplierEmail = createSupplierDto.SupplierEmail,
            SupplierPhone = createSupplierDto.SupplierPhone,
            SupplierAddress = createSupplierDto.SupplierAddress,
            IsActive = createSupplierDto.IsActive
        };

        _suppliersRepository.Add(supplier);

        await _suppliersRepository.SaveChanges();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for SupplierName {TargetSupplierName} and the supplier was created",
            authenticatedUserId, createSupplierDto.SupplierName);
        
        return supplier;
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