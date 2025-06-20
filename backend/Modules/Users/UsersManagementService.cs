using System.Text.Json;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class UsersManagementService(
    IUsersRepository _usersRepository,
    ILogger<UsersManagementService> _logger,
    IUsersQueryService _usersQueryService,
    IPriorityValidationService _priorityValidationService,
    IMailService _mailService,
    IHttpContextAccessor _httpContextAccessor,
    ITokenService _tokenService,
    IUsersBranchesQueryService _usersBranchesQueryServices,
    IGenericService<User> _genericService
    ) : IUsersManagementService
{
    public async Task<User> Create(User data)
    {
        return await _genericService.Create(data);
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        using var transaction = await _usersRepository.BeginTransactionAsync();

        try
        {
            foreach (Guid id in idsDto.Ids)
            {
                User? user = await _usersQueryService.GetById(id);
                if (user == null)
                {
                    ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound, Fields.Users.id, "User not found");
                    continue;
                }
                
                if (!await _usersBranchesQueryServices.EnsureUserCanModifyUserNotThrows(authenticatedUserId, id))
                {
                    ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.BranchNotMatched, Fields.Users.id,
                        "Not have the permissions to deactivate that user in that branch");
                    continue;
                }

                if (!user.isActive)
                {
                    ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed, Fields.Users.id,
                        "User was already deactivated");
                    continue;
                }

                if (authenticatedUserId != user.id && !_priorityValidationService.ValidateUserPriority(user))
                {
                    ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotEnoughPriority, Fields.Users.id,
                        "Not have enough priority to deactivate that user");
                    continue;
                }

                user.isActive = false;
                await _tokenService.RevokeRefreshTokensByUserId(id);
                await _usersRepository.SaveChangesAsync();

                responseDto.Success.Add(new IdResponseStatusDto
                {
                    Id = id,
                    Status = ResponseStatus.Success,
                    Message = "User successfully deactivated"
                });
            }

            await transaction.CommitAsync();

            _logger.LogInformation(
                "User with Id {authenticatedUserId} processed DeactivateUsers request. Response: {responseDto}",
                authenticatedUserId, responseDto);

            return responseDto;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error processing DeactivateUsers request for UserId {authenticatedUserId}",
                authenticatedUserId);
            throw;
        }
    }

    public async Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<IdResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ActivateUser for UsersIds {TargetUsersIds}",
            authenticatedUserId, idsDto.Ids)
            // ELIMINAR DTOS
            ;

        foreach (Guid id in idsDto.Ids)
        {
            User? user = await _usersQueryService.GetById(id);
            if (user == null)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound, Fields.Users.id, "User not found");
                continue;
            }
            
            if (!await _usersBranchesQueryServices.EnsureUserCanModifyUserNotThrows(authenticatedUserId, id))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.BranchNotMatched, Fields.Users.id,
                    "Not have the permissions to activate that user in that branch");
                continue;
            }
            
            if (user.isActive)
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed, Fields.Users.id,
                    "User already activated");
                continue;
            }

            if (authenticatedUserId != user.id && !_priorityValidationService.ValidateUserPriority(user))
            {
                ResponsesHelper.AddFailedResponseDto(responseDto, id, ResponseStatus.NotEnoughPriority, Fields.Users.id,
                    "Not have enough priority to activate that user");
                continue;
            }

            user.isActive = true;
            await _usersRepository.SaveChangesAsync();
            responseDto.Success.Add(new IdResponseStatusDto
            {
                Id = id,
                Status = ResponseStatus.Success,
                Message = "User Activated"
            });
            await _mailService.SendReactivateAccountMailAsync(user.email);
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed ActivateUsers request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<User> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with Id {authenticatedUserId} requested ChangePassword", authenticatedUserId);

        // Get the user
        User user = await _usersQueryService.GetByIdThrowsNotFoundAsync(authenticatedUserId);

        if (!user.isActive)
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested ChangePassword but the user is already deactivated",
                authenticatedUserId);
            throw new BadRequestException(message: "The user is already deactivated.", field: Fields.Users.isActive);
        }

        // Check password
        if (HasherHelper.VerifyHash(changePasswordDto.ActualPassword, user.password) == false)
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested ChangePassword but the actual password is not correct",
                authenticatedUserId);
            throw new UnauthorizedException(message: "The actual password is not correct.", reason: Reasons.WrongPassword);
        }

        // Change password
        user.password = HasherHelper.HashString(changePasswordDto.NewPassword);

        // Save changes
        await _usersRepository.SaveChangesAsync();
        await _tokenService.RevokeRefreshTokensByUserId(authenticatedUserId);

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ChangePassword and the password was changed",
            authenticatedUserId);

        return user;
    }

    public async Task<User> Update(Guid id, JsonElement data)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        User user = await _usersQueryService.GetByIdThrowsNotFoundAsync(id);

        if (authenticatedUserId != id)
        {
            _priorityValidationService.ValidateUserPriorityThrowsForbiddenException(user);
            await _usersBranchesQueryServices.EnsureUserCanModifyUserThrows(authenticatedUserId, id);
        }

        await _genericService.Update(user, data);

        return user;
    }
}