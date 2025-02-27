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
    
    /// <summary> 
    /// Recibe the userId and returns the User object if exists
    /// </summary>
    /// <param name="id">UserId</param>
    /// <returns>User</returns>
    /// <exception cref="NotFoundException">If not exist a user with the id.</exception>>
    Task<User> GetByIdThrowsNotFoundAsync(Guid id);
    
    /// <summary>
    /// Retrieves a user by their unique identifier and throws an exception if the user is not found.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user with the specified identifier.</returns>
    /// <exception cref="NotFoundException">Thrown when no user with the given identifier is found.</exception>
    Task<User?> GetByUserName(string userName);
    
    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The user with the specified username, or null if not found.</returns>
    Task<User?> GetByEmail(string email);
    
    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>The user with the specified email, or null if not found.</returns>
    Task<bool> ExistUserByUserName(string userName);
    
    /// <summary>
    /// Checks whether a user exists with the specified username.
    /// </summary>
    /// <param name="userName">The username to check for existence.</param>
    /// <returns>True if a user with the specified username exists; otherwise, false.</returns>
    Task<bool> ExistUserByEmail(string email);
    
    /// <summary>
    /// Checks whether a user exists with the specified email address.
    /// </summary>
    /// <param name="email">The email address to check for existence.</param>
    /// <returns>True if a user with the specified email exists; otherwise, false.</returns>
    Task<User?> Create(CreateUserDto createUserDto);
    
    /// <summary>
    /// Creates a new user with the provided details.
    /// </summary>
    /// <param name="createUserDto">The DTO containing the details required to create a new user.</param>
    /// <returns>
    /// The newly created user, or null if the creation failed.
    /// </returns>
    Task<User> GetByUserNameThrowsNotFound(string userName);
    
    /// <summary>
    /// Retrieves a user by their username and throws an exception if the user is not found.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <returns>The user with the specified username.</returns>
    /// <exception cref="NotFoundException">Thrown when no user with the given username is found.</exception>
    Task<User> DeactivateUserAsync(Guid id);
    
    /// <summary>
    /// Deactivates a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to deactivate.</param>
    /// <returns>The deactivated user.</returns>
    Task<User> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
    
    /// <summary>
    /// Changes the password for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose password is to be changed.</param>
    /// <param name="changePasswordDto">The DTO containing the current and new passwords.</param>
    /// <returns>The updated user with the new password.</returns>
    Task<User> UpdateAsync(UpdateUserDto updateUserDto);
    
    /// <summary>
    /// Updates the details of an existing user.
    /// </summary>
    /// <param name="updateUserDto">The DTO containing the updated user details.</param>
    /// <returns>The updated user.</returns>
}

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenService _tokenService;
    private readonly IGenericServie<User> _genericService;
    private readonly ILogger<UsersService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Guid AuthenticatedUserId =>
        Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   Guid.Empty.ToString());

    public UsersService(IUsersRepository usersRepository, IGenericServie<User> genericService,
        ITokenService tokenService, ILogger<UsersService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _usersRepository = usersRepository;
        _genericService = genericService;
        _tokenService = tokenService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    /// <summary>
    /// Updates the user information.
    /// </summary>
    /// <param name="updateUserDto">Data transfer object containing user update information.</param>
    /// <returns>Updated user.</returns>
    public async Task<User> UpdateAsync(UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested UpdateUser with UserId {TargetUserId}",
            AuthenticatedUserId, updateUserDto.UserId);

        // Get the user
        User user = await this.GetByIdThrowsNotFoundAsync(updateUserDto.UserId);

        bool hasChanges = false;

        // Update user
        if (updateUserDto.UserFirstName != null)
        {
            user.UserFirstName = updateUserDto.UserFirstName;
            hasChanges = true;
        }

        if (updateUserDto.UserLastName != null)
        {
            user.UserLastName = updateUserDto.UserLastName;
            hasChanges = true;
        }

        if (updateUserDto.UserUserName != null)
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
        
        if (updateUserDto.UserEmail != null)
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
    /// <summary>
    /// Changes the password of a user.
    /// </summary>
    /// <param name="userId">ID of the user.</param>
    /// <param name="changePasswordDto">DTO containing current and new password.</param>
    /// <returns>Updated user with the new password.</returns>
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
        if (PasswordHelper.VerifyPassword(changePasswordDto.ActualPassword, user.UserPassword) == false)
        {
            _logger.LogInformation(
                "User with Id {AuthenticatedUserId} requested ChangePassword but the actual password is not correct",
                AuthenticatedUserId);
            throw new UnauthorizedException(message: "The actual password is not correct.", reason: "WrongPassword");
        }

        // Change password
        user.UserPassword = PasswordHelper.HashPassword(changePasswordDto.NewPassword);

        // Save changes
        await this._usersRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested ChangePassword and the password was changed",
            AuthenticatedUserId);

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
            UserPassword = PasswordHelper.HashPassword(createUserDto.UserPassword),
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