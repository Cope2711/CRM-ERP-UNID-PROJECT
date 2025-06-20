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
                    Fields.UsersRoles.id, "Relation not exists"); continue;
            }

            if (!await _usersBranchesQueryService.EnsureUserCanModifyUserNotThrows(authenticatedUserId, userRole.userId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userRoleId, ResponseStatus.BranchNotMatched,
                    Fields.Users.id, "Not have branch to revoke role to that user"); continue;
            }
            
            if (!await _priorityValidationService.ValidateRolePriorityById(userRole.roleId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userRoleId, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.id, "Not have enough priority to modify that role"); continue;
            }
            
            if (!await _priorityValidationService.ValidateUserPriorityById(userRole.userId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, userRoleId, ResponseStatus.NotEnoughPriority,
                    Fields.Users.id, "Not have enough priority to modify that user"); continue;
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

    public async Task<ResponsesDto<ModelAndAssignResponseStatusDto>> AssignRolesToUsersAsync(ModelsAndAssignsDtos modelsAndAssignsDtos)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<ModelAndAssignResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignRolesToUsers with the object {UsersAndRolesDto}",
            authenticatedUserId, modelsAndAssignsDtos); 

        foreach (ModelAssignIdsDto modelAssignIds in modelsAndAssignsDtos.ModelAssignIds)
        {
            if (await _usersRolesQueryService.IsRoleAssignedToUserAsync(modelAssignIds.ModelId, modelAssignIds.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.AlreadyProcessed,
                    Fields.Roles.id, "Role already assigned to user"); continue;
            }
            
            if (!await _usersQueryService.ExistById(modelAssignIds.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.NotFound,
                    Fields.Users.id, "User not exist"); continue;
            }

            if (!await _rolesQueryService.ExistById(modelAssignIds.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.NotFound,
                    Fields.Roles.id, "Role not exist"); continue;
            }
            
            if (!await _usersBranchesQueryService.EnsureUserCanModifyUserNotThrows(authenticatedUserId, modelAssignIds.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.BranchNotMatched,
                    Fields.Users.id, "Not have branch to assign role to that user"); continue;
            }
            
            if (!await _priorityValidationService.ValidateRolePriorityById(modelAssignIds.AssignId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.id, "Not have enough priority to modify that role"); continue;
            }
            
            if (!await _priorityValidationService.ValidateUserPriorityById(modelAssignIds.ModelId))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, modelAssignIds, ResponseStatus.NotEnoughPriority,
                    Fields.Users.id, "Not have enough priority to modify that user"); continue;
            }

            // Add to database
            UserRole userRole = new UserRole
            {
                userId = modelAssignIds.ModelId,
                roleId = modelAssignIds.AssignId
            };

            _usersRolesRepository.Add(userRole);
            await _usersRolesRepository.SaveChangesAsync();

            responseDto.Success.Add(new ModelAndAssignResponseStatusDto
            {
                ModelAssignIds = modelAssignIds,
                Status = ResponseStatus.Success,
                Message = "RoleAssigned"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignRolesToUsers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }
}