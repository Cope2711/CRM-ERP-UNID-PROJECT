using System.Security.Claims;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersService
{
    Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto);
    Task<User> GetByIdThrowsNotFoundAsync(Guid id);
    Task<User?> GetById(Guid id);
    Task<User?> GetByUserName(string userName);
    Task<User?> GetByEmail(string email);
    Task<bool> ExistByIdThrowsNotFound(Guid id);
    Task<bool> ExistById(Guid id);
    Task<bool> ExistByUserName(string userName);
    Task<bool> ExistByEmail(string email);
    Task<User?> Create(CreateUserDto createUserDto);
    Task<User> GetByUserNameThrowsNotFound(string userName);
    Task<ResponsesDto<UserResponseStatusDto>> DeactivateUsersAsync(UsersIdsDto usersIdsDto);
    Task<User> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<User> UpdateAsync(UpdateUserDto updateUserDto);
    Task<ResponsesDto<UserResponseStatusDto>> ActivateUsersAsync(UsersIdsDto usersIdsDto);
}

public class UsersService(
    IUsersRepository _usersRepository,
    IGenericServie<User> _genericService,
    ITokenService _tokenService, 
    ILogger<UsersService> _logger, 
    IHttpContextAccessor _httpContextAccessor,
    IMailService _mailService, 
    IPriorityValidationService _priorityValidationService
    ) : IUsersService
{
    public async Task<User> UpdateAsync(UpdateUserDto updateUserDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateUser with UserId {TargetUserId}",
            authenticatedUserId, updateUserDto.UserId);

        User user = await GetByIdThrowsNotFoundAsync(updateUserDto.UserId);

        if (authenticatedUserId != updateUserDto.UserId)
            _priorityValidationService.ValidateUserPriorityThrowsForbiddenException(user);

        bool hasChanges = ModelsHelper.UpdateModel(user, updateUserDto, async (field, value) =>
        {
            return field switch
            {
                nameof(updateUserDto.UserEmail) => await ExistByEmail((string)value),
                nameof(updateUserDto.UserUserName) => await ExistByUserName((string)value),
                _ => false
            };
        });

        if (hasChanges)
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} updated User with Id {UpdatedUserId}",
                authenticatedUserId, user.UserId);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateUser but no changes were made",
                authenticatedUserId);
        }

        return user;
    }

    public async Task<User> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with Id {authenticatedUserId} requested ChangePassword", authenticatedUserId);

        // Get the user
        User user = await this.GetByIdThrowsNotFoundAsync(authenticatedUserId);

        if (!user.IsActive)
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested ChangePassword but the user is already deactivated",
                authenticatedUserId);
            throw new BadRequestException(message: "The user is already deactivated.", field: "IsActive");
        }

        // Check password
        if (HasherHelper.VerifyHash(changePasswordDto.ActualPassword, user.UserPassword) == false)
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested ChangePassword but the actual password is not correct",
                authenticatedUserId);
            throw new UnauthorizedException(message: "The actual password is not correct.", reason: "WrongPassword");
        }

        // Change password
        user.UserPassword = HasherHelper.HashString(changePasswordDto.NewPassword);

        // Save changes
        await _usersRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ChangePassword and the password was changed",
            authenticatedUserId);

        return user;
    }

    public async Task<ResponsesDto<UserResponseStatusDto>> ActivateUsersAsync(UsersIdsDto usersIdsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<UserResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested ActivateUser for UsersIds {TargetUsersIds}",
            authenticatedUserId, usersIdsDto.UsersIds);

        foreach (Guid id in usersIdsDto.UsersIds)
        {
            User? user = await GetById(id);
            if (user == null)
            {
                AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound, "UserId", "User not found"); continue;
            }

            if (user.IsActive)
            {
                AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed, "UserId", "User already activated"); continue;
            }
            
            if (authenticatedUserId != user.UserId && !_priorityValidationService.ValidateUserPriority(user))
            {
                AddFailedResponseDto(responseDto, id, ResponseStatus.NotEnoughPriority, "UserId", "Not have enough priority to activate that user"); continue;
            }
            
            user.IsActive = true;
            await _usersRepository.SaveChangesAsync();
            responseDto.Success.Add(new UserResponseStatusDto
            {
                UserId = id,
                Status = ResponseStatus.Success,
                Message = "User Activated"
            });
            await _mailService.SendReactivateAccountMailAsync(user.UserEmail);
        }
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} processed ActivateUsers request. Response: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<UserResponseStatusDto>> DeactivateUsersAsync(UsersIdsDto usersIdsDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        ResponsesDto<UserResponseStatusDto> responseDto = new();

        using var transaction = await _usersRepository.BeginTransactionAsync();

        try
        {
            foreach (Guid id in usersIdsDto.UsersIds)
            {
                User? user = await GetById(id);
                if (user == null)
                {
                    AddFailedResponseDto(responseDto, id, ResponseStatus.NotFound, "UserId", "User not found"); continue;
                }

                if (!user.IsActive)
                {
                    AddFailedResponseDto(responseDto, id, ResponseStatus.AlreadyProcessed, "UserId", "User was already deactivated"); continue;
                }
                
                if (authenticatedUserId != user.UserId && !_priorityValidationService.ValidateUserPriority(user))
                {
                    AddFailedResponseDto(responseDto, id, ResponseStatus.NotEnoughPriority, "UserId", "Not have enough priority to deactivate that user"); continue;
                }

                user.IsActive = false;
                await _tokenService.RevokeRefreshTokensByUserId(id);
                await _usersRepository.SaveChangesAsync();

                responseDto.Success.Add(new UserResponseStatusDto
                {
                    UserId = id,
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

    public async Task<User?> Create(CreateUserDto createUserDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);

        _logger.LogInformation("User with Id {authenticatedUserId} requested CreateUser with UserName {UserUserName}",
            authenticatedUserId, createUserDto.UserUserName);
        
        if (await ExistByUserName(createUserDto.UserUserName))
        {
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateUser but the user with username {UserUserName} already exists",
                authenticatedUserId, createUserDto.UserUserName);
            throw new UniqueConstraintViolationException(
                message: $"User with username {createUserDto.UserUserName} already exists", field: "UserUserName");
        }

        if (await ExistByEmail(createUserDto.UserEmail))
        {
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateUser but the user with email {UserEmail} already exists",
                authenticatedUserId, createUserDto.UserEmail);
            throw new UniqueConstraintViolationException(
                message: $"User with email {createUserDto.UserEmail} already exists", field: "UserEmail");
        }

        User user = new User
        {
            UserUserName = createUserDto.UserUserName,
            UserFirstName = createUserDto.UserFirstName,
            UserLastName = createUserDto.UserLastName,
            UserEmail = createUserDto.UserEmail,
            UserPassword = HasherHelper.HashString(createUserDto.UserPassword),
            IsActive = createUserDto.IsActive
        };

        _usersRepository.Add(user);
        await _usersRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateUser and the user was created with Id {CreatedUserId}",
            authenticatedUserId, user.UserId);

        return user;
    }

    public async Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto,
            query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _genericService.GetById(id, 
            query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id,
            query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await _genericService.GetFirstAsync(u => u.UserUserName, userName,
            query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User> GetByUserNameThrowsNotFound(string userName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(u => u.UserUserName, userName,
            query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<bool> ExistByIdThrowsNotFound(Guid id)
    {
        if (!await ExistById(id))
            throw new NotFoundException(message: $"User with id {id} not exist", field: "UserId");
        return true;
    }

    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(u => u.UserId, id);
    }

    
    public async Task<bool> ExistByUserName(string userName)
    {
        return await _genericService.ExistsAsync(u => u.UserUserName, userName);
    }

    public async Task<bool> ExistByEmail(string email)
    {
        return await _genericService.ExistsAsync(u => u.UserEmail, email);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _genericService.GetFirstAsync(u => u.UserEmail, email);
    }
    
    private void AddFailedResponseDto(ResponsesDto<UserResponseStatusDto> responseDto, Guid id, string status,
        string field, string message)
    {
        responseDto.Failed.Add(new UserResponseStatusDto
        {
            UserId = id,
            Status = status,
            Field = field,
            Message = message
        });
    }
}