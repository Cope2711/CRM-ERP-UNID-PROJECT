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
                    Fields.Branches.BranchId, "Branch not found");
                continue;
            }

            if (!await _usersBranchesQueryService.EnsureUserHasAccessToBranchNotThrows(authenticatedUserId,
                    branch.BranchId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId,
                    "Not have access to Branch");
                continue;
            }

            if (!branch.IsActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Branches.BranchId,
                    "Branch was already deactivated");
                continue;
            }

            branch.IsActive = false;
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
                    Fields.Branches.BranchId, "Branch not found");
                continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserHasAccessToBranchNotThrows(authenticatedUserId,
                    branch.BranchId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId,
                    "Not have access to Branch");
                continue;
            }

            if (branch.IsActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed,
                    Fields.Branches.BranchId,
                    "Branch was already activated");
                continue;
            }

            branch.IsActive = true;
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

    public async Task<Branch> Create(CreateBranchDto createBranchDto)
    {
        return await _genericService.Create(createBranchDto.ToModel());
    }

    public async Task<Branch> Update(Guid id, UpdateBranchDto updateBranchDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Branch branch = await _branchesQueryService.GetByIdThrowsNotFoundAsync(id);
        await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId, id);
        
        await _genericService.Update(branch, updateBranchDto);

        return branch;
    }
}