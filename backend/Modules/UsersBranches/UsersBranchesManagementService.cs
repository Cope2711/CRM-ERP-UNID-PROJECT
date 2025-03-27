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
    public async Task<ResponsesDto<UserBranchResponseStatusDto>> AssignBranchToUserAsync(UsersAndBranchesDtos usersAndBranchesDtos)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<UserBranchResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchToUser with the object {UserBranchDto}",
            authenticatedUserId, usersAndBranchesDtos);

        foreach (UserAndBranchIdDto userAndBranchIdDto in usersAndBranchesDtos.UserAndBranchIdDtos)
        {
            if (await _usersBranchesQueryService.IsUserAssignedToBranch(userAndBranchIdDto.UserId, userAndBranchIdDto.BranchId))
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.AlreadyProcessed,
                    Fields.Branches.BranchId, "User already assigned to branch"); continue;
            }
            
            if (!await _branchesQueryService.ExistById(userAndBranchIdDto.BranchId))
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.NotFound,
                    Fields.Branches.BranchId, "Branch not exist"); continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserHasAccessToBranchNotThrows(authenticatedUserId, userAndBranchIdDto.BranchId))
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId, "Not have the permissions to modify that user in that branch"); continue;
            }
            
            User? user = await _usersQueryService.GetById(userAndBranchIdDto.UserId);

            if (user == null)
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.NotFound,
                    Fields.Users.UserId, "User not exist"); continue;
            }
            
            if (!_priorityValidationService.ValidateUserPriority(user))
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.NotEnoughPriority,
                    Fields.Users.UserId, "Not have enough priority to modify that user"); continue;
            }
            
            // Add to database
            UserBranch userBranch = new UserBranch
            {
                UserId = userAndBranchIdDto.UserId,
                BranchId = userAndBranchIdDto.BranchId
            };

            _usersBranchesRepository.Add(userBranch);
            await _usersBranchesRepository.SaveChanges();

            responseDto.Success.Add(new UserBranchResponseStatusDto
            {
                UserAndBranchId = userAndBranchIdDto,
                Status = ResponseStatus.Success,
                Message = "User assigned to branch"
            });
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignBranchToUser and the result was {responseDto}",
            authenticatedUserId, responseDto);
        
        return responseDto;
    }
    
    public async Task<ResponsesDto<UserBranchResponseStatusDto>> RevokeBranchToUserAsync(UsersAndBranchesDtos usersAndBranchesDtos)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<UserBranchResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchToUser with the object {UserBranchDto}",
            authenticatedUserId, usersAndBranchesDtos);

        foreach (UserAndBranchIdDto userAndBranchIdDto in usersAndBranchesDtos.UserAndBranchIdDtos)
        {
            UserBranch? userBranch = await _usersBranchesQueryService.GetByUserIdAndBranchId(userAndBranchIdDto.UserId, userAndBranchIdDto.BranchId);

            if (userBranch == null)
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.NotFound,
                    Fields.Branches.BranchId, "User not assigned to branch"); continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserCanModifyUserNotThrows(authenticatedUserId, userAndBranchIdDto.UserId))
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.BranchNotMatched,
                    Fields.Branches.BranchId, "Not have the permissions to modify that user in that branch"); continue;
            }

            User? user = await _usersQueryService.GetById(userAndBranchIdDto.UserId);
            
            if (user == null)
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.NotFound,
                    Fields.Users.UserId, "User not exist"); continue;
            }
            
            if (!_priorityValidationService.ValidateUserPriority(user))
            {
                AddFailedResponseDto(responseDto, userAndBranchIdDto, ResponseStatus.NotEnoughPriority,
                    Fields.Users.UserId, "Not have enough priority to modify that user"); continue;
            }

            _usersBranchesRepository.Remove(userBranch);
            await _usersBranchesRepository.SaveChanges();

            responseDto.Success.Add(new UserBranchResponseStatusDto
            {
                UserAndBranchId = userAndBranchIdDto,
                Status = ResponseStatus.Success,
                Message = "User revoked from branch"
            });
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeBranchToUser and the result was {responseDto}",
            authenticatedUserId, responseDto);
        
        return responseDto;
    }
    
    private void AddFailedResponseDto(ResponsesDto<UserBranchResponseStatusDto> responseDto,
        UserAndBranchIdDto userAndBranchIdDto, string status, string field, string message)
    {
        responseDto.Failed.Add(new UserBranchResponseStatusDto{
            UserAndBranchId = userAndBranchIdDto,
            Status = status,
            Field = field,
            Message = message
        });
    }
}