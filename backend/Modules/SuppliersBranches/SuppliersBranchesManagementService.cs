using System.Text.Json;
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
    public async Task<SupplierBranch> Update(JsonElement data)
    {
        if (!data.TryGetProperty(Fields.SuppliersBranches.id, out JsonElement idElement) || 
            idElement.ValueKind != JsonValueKind.String || 
            !Guid.TryParse(idElement.GetString(), out Guid id))
        {
            throw new ArgumentException("Invalid or missing id in JSON data");
        }

        SupplierBranch supplierBranch = await _suppliersBranchesQueryService.GetByIdThrowsNotFound(id);

        await _genericService.Update(supplierBranch, data);

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
                    Fields.SuppliersBranches.branchId, "Branch already assigned to supplier");
                continue;
            }

            if (!await _suppliersQueryService.ExistById(modelAssignIdsDto.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Suppliers.id, "Supplier not exist");
                continue;
            }

            if (!await _branchesQueryService.ExistById(modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.NotFound,
                    Fields.Products.id, "Branch not exist");
                continue;
            }
            
            if (!await _usersBranchesQueryService.IsUserAssignedToBranch(authenticatedUserId, modelAssignIdsDto.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIdsDto, ResponseStatus.BranchNotMatched,
                    Fields.Branches.id, "User not assigned to branch");
                continue;
            }

            // Add to database
            SupplierBranch supplierBranch = new()
            {
                supplierId = modelAssignIdsDto.ModelId,
                branchId = modelAssignIdsDto.AssignId,
                createdDate = DateTime.UtcNow,
                updatedDate = DateTime.UtcNow
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
                    Fields.SuppliersBranches.id, "SupplierBranch not exist");
                continue;
            }
            
            if (!await _usersBranchesQueryService.IsUserAssignedToBranch(authenticatedUserId, supplierBranch.branchId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, supplierBranchId, ResponseStatus.BranchNotMatched,
                    Fields.Branches.id, "User not assigned to branch");
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