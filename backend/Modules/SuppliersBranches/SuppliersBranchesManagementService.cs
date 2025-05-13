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
    IUsersBranchesQueryService _usersBranchesQueryService,
    IGenericService<SupplierBranch> _genericService
) : ISuppliersBranchesManagementService
{
    public async Task<SupplierBranch> Update(UpdateSupplierBranchDto updateSupplierBranchDto)
    {
        SupplierBranch? supplierBranch = await _suppliersBranchesQueryService.GetByIdThrowsNotFound(updateSupplierBranchDto.SupplierBranchId);
        
        await _genericService.Update(supplierBranch, updateSupplierBranchDto);
        
        return supplierBranch;
    }
    
    public async Task<ResponsesDto<ModelAndAssignResponseStatusDto>> AssignBranchesToSuppliers(
        ModelsAndAssignsDtos modelsAndAssignsDtos)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<ModelAndAssignResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchesToSuppliers with the object {SuppliersAndBranchesDto}",
            authenticatedUserId, modelsAndAssignsDtos);

        foreach (ModelAssignIdsDto modelAssignIdsDto in modelsAndAssignsDtos.ModelAssignIds)
        {
            if (await _suppliersBranchesQueryService.IsSupplierAssignedToBranch(modelAssignIdsDto.ModelId,
                    modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.AlreadyProcessed,
                    Fields.SuppliersBranches.BranchId, "Branch already assigned to supplier");
                continue;
            }

            if (!await _suppliersQueryService.ExistById(modelAssignIdsDto.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Suppliers.SupplierId, "Supplier not exist");
                continue;
            }

            if (!await _branchesQueryService.ExistById(modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Products.ProductId, "Branch not exist");
                continue;
            }
            
            if (!await _usersBranchesQueryService.IsUserAssignedToBranch(authenticatedUserId, modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId, "User not assigned to branch");
                continue;
            }

            // Add to database
            SupplierBranch supplierBranch = new()
            {
                SupplierId = modelAssignIdsDto.ModelId,
                BranchId = modelAssignIdsDto.AssignId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _suppliersBranchesRepository.Add(supplierBranch);
            await _suppliersBranchesRepository.SaveChangesAsync();

            responseDto.Success.Add(new ModelAndAssignResponseStatusDto
            {
                ModelAssignIds = modelAssignIdsDto,
                Status = ResponseStatus.Success,
                Message = "BranchAssigned"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchesToSuppliers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }
    
    public async Task<ResponsesDto<IdResponseStatusDto>> RevokeBranchesFromSuppliers(
        IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchesFromSuppliers with the object {SuppliersBranchesIdsDto}",
            authenticatedUserId, idsDto);

        foreach (Guid supplierBranchId in idsDto.Ids)
        {
            // Exist?
            SupplierBranch? supplierBranch =
                await _suppliersBranchesQueryService.GetById(supplierBranchId);

            if (supplierBranch == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, supplierBranchId, ResponseStatus.NotFound,
                    Fields.SuppliersBranches.SupplierBranchId, "SupplierBranch not exist");
                continue;
            }
            
            if (!await _usersBranchesQueryService.IsUserAssignedToBranch(authenticatedUserId, supplierBranch.BranchId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, supplierBranchId, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId, "User not assigned to branch");
                continue;
            }
            
            _suppliersBranchesRepository.Remove(supplierBranch);
            await _suppliersBranchesRepository.SaveChangesAsync();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = supplierBranchId,
                Status = ResponseStatus.Success,
                Message = "BranchRevoked"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchesFromSuppliers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }
}