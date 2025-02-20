using System.Security.Claims;
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
    Task<User?> GetByUserName(string userName);
    Task<User?> GetByEmail(string email);
    Task<bool> ExistUserByUserName(string userName);
    Task<bool> ExistUserByEmail(string email);
    Task<User?> Create(CreateUserDto createUserDto);
    Task<User> GetByUserNameThrowsNotFound(string userName);
    Task<User> DeactivateUserAsync(Guid id);
    Task<User> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
    Task<User> UpdateAsync(UpdateUserDto updateUserDto);
    Task<User> ActivateUserAsync(Guid id);
}

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenService _tokenService;
    private readonly IGenericServie<User> _genericService;
    private readonly ILogger<UsersService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailService _mailService;

    private Guid AuthenticatedUserId =>
        Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   Guid.Empty.ToString());

    public UsersService(IUsersRepository usersRepository, IGenericServie<User> genericService,
        ITokenService tokenService, ILogger<UsersService> logger, IHttpContextAccessor httpContextAccessor, IMailService mailService)
    {
        _usersRepository = usersRepository;
        _genericService = genericService;
        _tokenService = tokenService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _mailService = mailService;
    }
    
    public async Task<User> UpdateAsync(UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested UpdateUser with UserId {TargetUserId}",
            AuthenticatedUserId, updateUserDto.UserId);

        // Get the user
        User user = await this.GetByIdThrowsNotFoundAsync(updateUserDto.UserId);

        bool hasChanges = false;

        // Update user
        if (updateUserDto.UserFirstName != null && updateUserDto.UserFirstName != user.UserFirstName)
        {
            user.UserFirstName = updateUserDto.UserFirstName;
            hasChanges = true;
        }

        if (updateUserDto.UserLastName != null && updateUserDto.UserLastName != user.UserLastName)
        {
            user.UserLastName = updateUserDto.UserLastName;
            hasChanges = true;
        }

        if (updateUserDto.UserUserName != null && updateUserDto.UserEmail != user.UserUserName)
        {
            if (await ExistUserByUserName(updateUserDto.UserUserName))
            {
                _logger.LogError(
                    "User with Id {AuthenticatedUserId} requested UpdateUser but the user with username {UserUserName} already exists",
                    AuthenticatedUserId, updateUserDto.UserUserName);
                throw new UniqueConstraintViolationException(
                    message: $"User with username {updateUserDto.UserUserName} already exists", field: "UserUserName");
            }

            user.UserUserName = updateUserDto.UserUserName;
            hasChanges = true;
        }
        
        if (updateUserDto.UserEmail != null && updateUserDto.UserEmail != user.UserEmail)
        {
            // Check if the email is already in use
            if (await ExistUserByEmail(updateUserDto.UserEmail))
            {
                _logger.LogError(
                    "User with Id {AuthenticatedUserId} requested UpdateUser but the user with email {UserEmail} already exists",
                    AuthenticatedUserId, updateUserDto.UserEmail);
                throw new UniqueConstraintViolationException(
                    message: $"User with email {updateUserDto.UserEmail} already exists", field: "UserEmail");
            }

            user.UserEmail = updateUserDto.UserEmail;
            hasChanges = true;
        }

        if (hasChanges)
        {
            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested UpdateUser and the user was updated with Id {UpdatedUserId}",
                AuthenticatedUserId, user.UserId);
            await this._usersRepository.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested UpdateUser and the user was not updated with Id {UpdatedUserId}",
                AuthenticatedUserId, updateUserDto.UserId);
        }
        
        return user;
    }

    public async Task<User> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested ChangePassword", AuthenticatedUserId);

        // Get the user
        User user = await this.GetByIdThrowsNotFoundAsync(userId);

        if (user.IsActive == false)
        {
            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested ChangePassword but the user is already deactivated",
                AuthenticatedUserId);
            throw new BadRequestException(message: "The user is already deactivated.", field: "IsActive");
        }

        // Check password
        if (HasherHelper.VerifyHash(changePasswordDto.ActualPassword, user.UserPassword) == false)
        {
            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested ChangePassword but the actual password is not correct",
                AuthenticatedUserId);
            throw new UnauthorizedException(message: "The actual password is not correct.", reason: "WrongPassword");
        }

        // Change password
        user.UserPassword = HasherHelper.HashString(changePasswordDto.NewPassword);

        // Save changes
        await this._usersRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested ChangePassword and the password was changed",
            AuthenticatedUserId);

        return user;
    }

    public async Task<User> ActivateUserAsync(Guid id)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested ActivateUser for UserId {TargetUserId}",
            AuthenticatedUserId, id);
        
        // Get the user
        User user = await this.GetByIdThrowsNotFoundAsync(id);

        if (user.IsActive == true)
        {
            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested ActivateUser for UserId {TargetUserId} but the user is already active",
                AuthenticatedUserId, id);
            throw new BadRequestException(message: "The user is already active.", field: "IsActive");
        }
        
        user.IsActive = true;

        // Save changes
        await this._usersRepository.SaveChangesAsync();

        await _mailService.SendReactivateAccountMailAsync(user.UserEmail);
        
        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested ActivateUser for UserId {TargetUserId} and the user was activated",
            AuthenticatedUserId, id);
        
        return user;
    }
    
    public async Task<User> DeactivateUserAsync(Guid id)
    {
        using var transaction = await _usersRepository.BeginTransactionAsync();

        try
        {
            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested DeactivateUser for UserId {TargetUserId}",
                AuthenticatedUserId, id);

            // Deactivate user
            User user = await this.GetByIdThrowsNotFoundAsync(id);

            if (user.IsActive == false)
            {
                _logger.LogInformation(
                    "User with Id {AuthenticatedUserId} requested DeactivateUser for UserId {TargetUserId} but the user is already deactivated",
                    AuthenticatedUserId, id);
                throw new BadRequestException(message: "The user is already deactivated.", field: "IsActive");
            }

            user.IsActive = false;

            // Inactivate resfresh tokens
            await _tokenService.RevokeRefreshsTokensByUserId(user.UserId);

            // Save changes
            await this._usersRepository.SaveChangesAsync();

            await transaction.CommitAsync();

            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested DeactivateUser for UserId {TargetUserId} and the user was deactivated",
                AuthenticatedUserId, id);

            return user;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto,
            query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _genericService.GetById(id, query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
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

    public async Task<bool> ExistUserByUserName(string userName)
    {
        return await _genericService.ExistsAsync(u => u.UserUserName, userName);
    }

    public async Task<bool> ExistUserByEmail(string email)
    {
        return await _genericService.ExistsAsync(u => u.UserEmail, email);
    }
    
    public async Task<User?> GetByEmail(string email)
    {
        return await _genericService.GetFirstAsync(u => u.UserEmail, email);
    }

    public async Task<User?> Create(CreateUserDto createUserDto)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested CreateUser with UserName {UserUserName}",
            AuthenticatedUserId, createUserDto.UserUserName);
        if (await ExistUserByUserName(createUserDto.UserUserName))
        {
            _logger.LogError(
                "User with Id {AuthenticatedUserId} requested CreateUser but the user with username {UserUserName} already exists",
                AuthenticatedUserId, createUserDto.UserUserName);
            throw new UniqueConstraintViolationException(
                message: $"User with username {createUserDto.UserUserName} already exists", field: "UserUserName");
        }

        if (await ExistUserByEmail(createUserDto.UserEmail))
        {
            _logger.LogError(
                "User with Id {AuthenticatedUserId} requested CreateUser but the user with email {UserEmail} already exists",
                AuthenticatedUserId, createUserDto.UserEmail);
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

        this._usersRepository.Add(user);
        await this._usersRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested CreateUser and the user was created with Id {CreatedUserId}",
            AuthenticatedUserId, user.UserId);

        return user;
    }
}