﻿using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class UsersRolesService(
    IUsersQueryService _usersQueryService,
    IUsersRolesRepository _usersRolesRepository,
    IGenericService<UserRole> _genericService,
    ILogger<UsersRolesService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IPriorityValidationService _priorityValidationService,
    IRolesQueryService _rolesQueryService
) : IUsersRolesService
{
    public async Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto,
            query => query.Include(ur => ur.User).Include(ur => ur.Role));
    }

    public async Task<UserRole> GetByUserIdAndRoleIdThrowsNotFoundAsync(Guid userId, Guid roleId)
    {
        UserRole? userRole = await GetByUserIdAndRoleId(userId, roleId);
        if (userRole == null)
            throw new NotFoundException("This role is not assigned to the user.", field: Fields.Roles.RoleId);
        return userRole;
    }

    public async Task<UserRole?> GetByUserIdAndRoleId(Guid userId, Guid roleId)
    {
        return await _usersRolesRepository.GetByUserIdAndRoleIdAsync(userId, roleId);
    }

    public async Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId)
    {
        return await _usersRolesRepository.IsRoleAssignedToUserAsync(userId, roleId);
    }

    public async Task<ResponsesDto<UserAndRoleResponseStatusDto>> RevokeRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<UserAndRoleResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRolesToUsers with the object {UsersAndRolesDto}",
            authenticatedUserId, usersAndRolesDto);

        foreach (UserAndRoleIdDto userAndRoleIdDto in usersAndRolesDto.UserAndRoleId)
        {
            UserRole? userRole = await GetByUserIdAndRoleId(userAndRoleIdDto.UserId, userAndRoleIdDto.RoleId);

            if (userRole == null)
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotFound,
                    Fields.UsersRoles.UserRoleId, "Relation not exists"); continue;
            }

            // User has enough priority to revoke this role from this user?
            Role role = await _rolesQueryService.GetByIdThrowsNotFoundAsync(userAndRoleIdDto.RoleId);
            if (!_priorityValidationService.ValidateRolePriority(role))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.RoleId, "Not have enough priority to modify that role"); continue;
            }
            
            if (authenticatedUserId != userAndRoleIdDto.UserId)
            {
                User user = await _usersQueryService.GetByIdThrowsNotFoundAsync(userAndRoleIdDto.UserId);
                if (!_priorityValidationService.ValidateUserPriority(user))
                {
                    AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotEnoughPriority,
                        Fields.Users.UserId, "Not have enough priority to modify that user"); continue;
                }
            }

            _usersRolesRepository.Remove(userRole);
            await _usersRolesRepository.SaveChangesAsync();

            responseDto.Success.Add(new UserAndRoleResponseStatusDto
            {
                UserAndRoleId = userAndRoleIdDto,
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
            if (await IsRoleAssignedToUserAsync(userAndRoleIdDto.UserId, userAndRoleIdDto.RoleId))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.AlreadyProcessed,
                    Fields.Roles.RoleId, "Role already assigned to user"); continue;
            }

            Role? role = await _rolesQueryService.GetById(userAndRoleIdDto.RoleId);
            User? user = await _usersQueryService.GetById(userAndRoleIdDto.UserId);

            if (user == null)
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotFound,
                    Fields.Users.UserId, "User not exist"); continue;
            }

            if (role == null)
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotFound,
                    Fields.Roles.RoleId, "Role not exist"); continue;
            }
            
            if (!_priorityValidationService.ValidateRolePriority(role))
            {
                AddFailedResponseDto(responseDto, userAndRoleIdDto, ResponseStatus.NotEnoughPriority,
                    Fields.Roles.RoleId, "Not have enough priority to modify that role"); continue;
            }
            
            if (authenticatedUserId != userAndRoleIdDto.UserId && !_priorityValidationService.ValidateUserPriority(user))
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