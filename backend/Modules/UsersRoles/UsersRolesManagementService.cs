using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class UsersRolesManagementService(
    IUsersQueryService _usersQueryService,
    IUsersRolesQueryService _usersRolesQueryService,
    IUsersRolesRepository _usersRolesRepository,
    ILogger<UsersRolesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IPriorityValidationService _priorityValidationService,
    IRolesQueryService _rolesQueryService,
    IUsersBranchesQueryService _usersBranchesQueryService
) : IUsersRolesManagementService
{
    public async Task<ResponsesDto<IdResponseStatusDto>> RevokeRolesToUsersAsync(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRolesToUsers with the object {UsersAndRolesDto}",
            authenticatedUserId, idsDto);

        foreach (Guid userRoleId in idsDto.Ids)
        {
            UserRole? userRole = await _usersRolesQueryService.GetById(userRoleId);
            
            if (userRole == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userRoleId, ResponseStatus.NotFound,
                    Fields.UsersRoles.UserRoleId, "Relation not exists"); continue;
            }

            if (!await _usersBranchesQueryService.EnsureUserCanModifyUserNotThrows(authenticatedUserId, userRole.UserId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userRoleId, ResponseStatus.BranchNotMatched,
                    Fields.Users.UserId, "Not have branch to revoke role to that user"); continue;
            }
            
            if (!await _priorityValidationService.ValidateRolePriorityById(userRole.RoleId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userRoleId, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.RoleId, "Not have enough priority to modify that role"); continue;
            }
            
            if (!await _priorityValidationService.ValidateUserPriorityById(userRole.UserId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userRoleId, ResponseStatus.NotEnoughPriority,
                    Fields.Users.UserId, "Not have enough priority to modify that user"); continue;
            }
            
            _usersRolesRepository.Remove(userRole);
            await _usersRolesRepository.SaveChangesAsync();

            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = userRoleId,
                Status = ResponseStatus.Success,
                Message = "Role revoked"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRolesToUsers and the result was {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<UserAndRoleResponseStatusDto>> AssignRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<UserAndRoleResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignRolesToUsers with the object {UsersAndRolesDto}",
            authenticatedUserId, usersAndRolesDto); 

        foreach (UserAndRoleIdDto userAndRoleIdDto in usersAndRolesDto.UserAndRoleId)
        {
            if (await _usersRolesQueryService.IsRoleAssignedToUserAsync(userAndRoleIdDto.UserId, userAndRoleIdDto.RoleId))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.AlreadyProcessed,
                    Fields.Roles.RoleId, "Role already assigned to user"); continue;
            }
            
            if (!await _usersQueryService.ExistById(userAndRoleIdDto.UserId))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotFound,
                    Fields.Users.UserId, "User not exist"); continue;
            }

            if (!await _rolesQueryService.ExistById(userAndRoleIdDto.RoleId))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotFound,
                    Fields.Roles.RoleId, "Role not exist"); continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserCanModifyUserNotThrows(authenticatedUserId, userAndRoleIdDto.UserId))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.BranchNotMatched,
                    Fields.Users.UserId, "Not have branch to assign role to that user"); continue;
            }
            
            if (!await _priorityValidationService.ValidateRolePriorityById(userAndRoleIdDto.RoleId))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.RoleId, "Not have enough priority to modify that role"); continue;
            }
            
            if (!await _priorityValidationService.ValidateUserPriorityById(userAndRoleIdDto.UserId))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotEnoughPriority,
                    Fields.Users.UserId, "Not have enough priority to modify that user"); continue;
            }

            // Add to database
            UserRole userRole = new UserRole
            {
                UserId = userAndRoleIdDto.UserId,
                RoleId = userAndRoleIdDto.RoleId
            };

            _usersRolesRepository.Add(userRole);
            await _usersRolesRepository.SaveChangesAsync();

            responseDto.Success.Add(new UserAndRoleResponseStatusDto
            {
                UserAndRoleId = userAndRoleIdDto,
                Status = ResponseStatus.Success,
                Message = "RoleAssigned"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignRolesToUsers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    private void AddFailedResponseDto(ResponsesDto<UserAndRoleResponseStatusDto> responseDto,
        UserAndRoleIdDto userAndRoleIdDto, string status, string field, string message)
    {
        responseDto.Failed.Add(new UserAndRoleResponseStatusDto{
            UserAndRoleId = userAndRoleIdDto,
            Status = status,
            Field = field,
            Message = message
        });
    }
}