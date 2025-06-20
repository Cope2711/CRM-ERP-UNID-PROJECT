using System.Text.Json;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class BranchesManagementService(
    IBranchesQueryService _branchesQueryService,
    IBranchesRepository _branchesRepository,
    ILogger<BranchesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IUsersBranchesQueryService _usersBranchesQueryService,
    IGenericService<Branch> _genericService
    ) : IBranchesManagementService
{
    public async Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Branch? branch = await _branchesQueryService.GetById(id);
            if (branch == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Branches.id, "Branch not found");
                continue;
            }

            if (!await _usersBranchesQueryService.EnsureUserHasAccessToBranchNotThrows(authenticatedUserId,
                    branch.id))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.BranchNotMatched,
                    Fields.Branches.id,
                    "Not have access to Branch");
                continue;
            }

            if (!branch.isActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Branches.id,
                    "Branch was already deactivated");
                continue;
            }

            branch.isActive = false;
            await _branchesRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Branch successfully deactivated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Deactivate Branches request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        foreach (Guid id in idsDto.Ids)
        {
            Branch? branch = await _branchesQueryService.GetById(id);
            if (branch == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound,
                    Fields.Branches.id, "Branch not found");
                continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserHasAccessToBranchNotThrows(authenticatedUserId,
                    branch.id))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.BranchNotMatched,
                    Fields.Branches.id,
                    "Not have access to Branch");
                continue;
            }

            if (branch.isActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Branches.id,
                    "Branch was already activated");
                continue;
            }

            branch.isActive = true;
            await _branchesRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "Branch successfully activated"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed Activate Branches request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<Branch> Create(Branch data)
    {
        return await _genericService.Create(data);
    }

    public async Task<Branch> Update(Guid id, JsonElement data)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Branch branch = await _branchesQueryService.GetByIdThrowsNotFoundAsync(id);
        await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId, id);
        
        await _genericService.Update(branch, data);
        
        return branch;
    }
}