using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class UsersBranchesManagementService(
    IUsersBranchesRepository _usersBranchesRepository,
    IUsersBranchesQueryService _usersBranchesQueryService,
    ILogger<UsersBranchesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IUsersQueryService _usersQueryService,
    IBranchesQueryService _branchesQueryService,
    IPriorityValidationService _priorityValidationService
) : IUsersBranchesManagementService
{
    public async Task<ResponsesDto<ModelAndAssignResponseStatusDto>> AssignBranchToUser(ModelsAndAssignsDtos modelsAndAssignsDtos)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<ModelAndAssignResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchToUser with the object {UserBranchDto}",
            authenticatedUserId, modelsAndAssignsDtos);

        foreach (ModelAssignIdsDto modelAssignIds in modelsAndAssignsDtos.ModelAssignIds)
        {
            if (await _usersBranchesQueryService.IsUserAssignedToBranch(modelAssignIds.ModelId, modelAssignIds.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.AlreadyProcessed,
                    Fields.Branches.id, "User already assigned to branch"); continue;
            }
            
            if (!await _branchesQueryService.ExistById(modelAssignIds.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.NotFound,
                    Fields.Branches.id, "Branch not exist"); continue;
            }
            
            if (!await _usersQueryService.ExistById(modelAssignIds.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.NotFound,
                    Fields.Users.id, "User not exist"); continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserHasAccessToBranchNotThrows(authenticatedUserId, modelAssignIds.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.BranchNotMatched,
                    Fields.Branches.id, "Not have the permissions to modify that user in that branch"); continue;
            }
            
            if (!await _priorityValidationService.ValidateUserPriorityById(modelAssignIds.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.NotEnoughPriority,
                    Fields.Users.id, "Not have enough priority to modify that user"); continue;
            }
            
            // Add to database
            UserBranch userBranch = new UserBranch
            {
                userId = modelAssignIds.ModelId,
                branchId = modelAssignIds.AssignId
            };

            _usersBranchesRepository.Add(userBranch);
            await _usersBranchesRepository.SaveChanges();

            responseDto.Success.Add(new ModelAndAssignResponseStatusDto
            {
                ModelAssignIds = modelAssignIds,
                Status = ResponseStatus.Success,
                Message = "User assigned to branch"
            });
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchToUser and the result was {responseDto}",
            authenticatedUserId, responseDto);
        
        return responseDto;
    }
    
    public async Task<ResponsesDto<IdResponseStatusDto>> RevokeBranchToUser(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchToUser with the object {UserBranchDto}",
            authenticatedUserId, idsDto);

        foreach (Guid userBranchId in idsDto.Ids)
        {
            UserBranch? userBranch = await _usersBranchesQueryService.GetById(userBranchId);

            if (userBranch == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userBranchId, ResponseStatus.NotFound,
                    Fields.Branches.id, "User not assigned to branch"); continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserCanModifyUserNotThrows(authenticatedUserId, userBranch.userId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userBranchId, ResponseStatus.BranchNotMatched,
                    Fields.Branches.id, "Not have the permissions to modify that user in that branch"); continue;
            }
            
            if (!await _priorityValidationService.ValidateUserPriorityById(userBranch.userId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userBranchId, ResponseStatus.NotEnoughPriority,
                    Fields.Users.id, "Not have enough priority to modify that user"); continue;
            }

            _usersBranchesRepository.Remove(userBranch);
            await _usersBranchesRepository.SaveChanges();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = userBranchId,
                Status = ResponseStatus.Success,
                Message = "User revoked from branch"
            });
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchToUser and the result was {responseDto}",
            authenticatedUserId, responseDto);
        
        return responseDto;
    }
}