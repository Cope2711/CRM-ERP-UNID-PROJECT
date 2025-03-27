using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class SuppliersBranchesManagementService(
    ILogger<SuppliersBranchesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    ISuppliersBranchesQueryService _suppliersBranchesQueryService,
    ISuppliersBranchesRepository _suppliersBranchesRepository,
    ISuppliersQueryService _suppliersQueryService,
    IBranchesQueryService _branchesQueryService,
    IUsersBranchesQueryService _usersBranchesQueryService
) : ISuppliersBranchesManagementService
{
    public async Task<SupplierBranch> Update(UpdateSupplierBranchDto updateSupplierBranchDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateSupplierBranch with SupplierBranchId {TargetSupplierBranchId}",
            authenticatedUserId, updateSupplierBranchDto.SupplierBranchId);
        
        SupplierBranch? supplierBranch = await _suppliersBranchesQueryService.GetByIdThrowsNotFound(updateSupplierBranchDto.SupplierBranchId);
        
        bool hasChanges = ModelsHelper.UpdateModel(supplierBranch, updateSupplierBranchDto, async (field, value) =>
        {
            return field switch
            {
                nameof(updateSupplierBranchDto.IsPreferredSupplier) => value == null,
                _ => false
            };
        });
        
        if (hasChanges)
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} updated SupplierBranch with Id {UpdatedSupplierBranchId}",
                authenticatedUserId, supplierBranch.SupplierBranchId);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateSupplierBranch but no changes were made",
                authenticatedUserId);
        }
        
        return supplierBranch;
    }
    
    public async Task<ResponsesDto<SuppliersBranchResponseStatusDto>> AssignBranchesToSuppliers(
        SuppliersAndBranchesDto suppliersAndBranchesDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<SuppliersBranchResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchesToSuppliers with the object {SuppliersAndBranchesDto}",
            authenticatedUserId, suppliersAndBranchesDto);

        foreach (SupplerAndBranchIdDto supplierAndBranchIdDto in suppliersAndBranchesDto.SupplerAndBranchIdDto)
        {
            if (await _suppliersBranchesQueryService.IsSupplierAssignedToBranch(supplierAndBranchIdDto.SupplierId,
                    supplierAndBranchIdDto.BranchId))
            {
                AddFailedResponseDto(responseDto, supplierAndBranchIdDto, ResponseStatus.AlreadyProcessed,
                    Fields.SuppliersBranches.BranchId, "Branch already assigned to supplier");
                continue;
            }

            if (!await _suppliersQueryService.ExistById(supplierAndBranchIdDto.SupplierId))
            {
                AddFailedResponseDto(responseDto, supplierAndBranchIdDto, ResponseStatus.NotFound,
                    Fields.Suppliers.SupplierId, "Supplier not exist");
                continue;
            }

            if (!await _branchesQueryService.ExistById(supplierAndBranchIdDto.BranchId))
            {
                AddFailedResponseDto(responseDto, supplierAndBranchIdDto, ResponseStatus.NotFound,
                    Fields.Products.ProductId, "Branch not exist");
                continue;
            }
            
            if (!await _usersBranchesQueryService.IsUserAssignedToBranch(authenticatedUserId, supplierAndBranchIdDto.BranchId))
            {
                AddFailedResponseDto(responseDto, supplierAndBranchIdDto, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId, "User not assigned to branch");
                continue;
            }

            // Add to database
            SupplierBranch supplierBranch = new()
            {
                SupplierId = supplierAndBranchIdDto.SupplierId,
                BranchId = supplierAndBranchIdDto.BranchId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _suppliersBranchesRepository.Add(supplierBranch);
            await _suppliersBranchesRepository.SaveChangesAsync();

            responseDto.Success.Add(new SuppliersBranchResponseStatusDto
            {
                SupplerAndBranchId = supplierAndBranchIdDto,
                Status = ResponseStatus.Success,
                Message = "BranchAssigned"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchesToSuppliers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }
    
    public async Task<ResponsesDto<SuppliersBranchesRevokedResponseStatusDto>> RevokeBranchesFromSuppliers(
        SuppliersBranchesIdsDto suppliersBranchesIdsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<SuppliersBranchesRevokedResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchesFromSuppliers with the object {SuppliersBranchesIdsDto}",
            authenticatedUserId, suppliersBranchesIdsDto);

        foreach (SupplierBranchIdDto supplierBranchIdDto in suppliersBranchesIdsDto.SupplierBranchIds)
        {
            // Exist?
            SupplierBranch? supplierBranch =
                await _suppliersBranchesQueryService.GetById(supplierBranchIdDto.SupplierBranchId);

            if (supplierBranch == null)
            {
                AddFailedResponseDto(responseDto, supplierBranchIdDto, ResponseStatus.NotFound,
                    Fields.SuppliersBranches.SupplierBranchId, "SupplierBranch not exist");
                continue;
            }
            
            if (!await _usersBranchesQueryService.IsUserAssignedToBranch(authenticatedUserId, supplierBranch.BranchId))
            {
                AddFailedResponseDto(responseDto, supplierBranchIdDto, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId, "User not assigned to branch");
                continue;
            }
            
            _suppliersBranchesRepository.Remove(supplierBranch);
            await _suppliersBranchesRepository.SaveChangesAsync();

            responseDto.Success.Add(new SuppliersBranchesRevokedResponseStatusDto
            {
                SupplierBranchId = supplierBranchIdDto,
                Status = ResponseStatus.Success,
                Message = "BranchRevoked"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchesFromSuppliers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public void AddFailedResponseDto(ResponsesDto<SuppliersBranchResponseStatusDto> responseDto,
        SupplerAndBranchIdDto supplierAndBranchIdDto, string status, string field, string message)
    {
        responseDto.Failed.Add(new SuppliersBranchResponseStatusDto
        {
            SupplerAndBranchId = supplierAndBranchIdDto,
            Status = status,
            Field = field,
            Message = message
        });
    }

    public void AddFailedResponseDto(ResponsesDto<SuppliersBranchesRevokedResponseStatusDto> responseDto,
        SupplierBranchIdDto supplierBranchIdDto, string status, string field, string message)
    {
        responseDto.Failed.Add(new SuppliersBranchesRevokedResponseStatusDto
        {
            SupplierBranchId = supplierBranchIdDto,
            Status = status,
            Field = field,
            Message = message
        });
    }
}