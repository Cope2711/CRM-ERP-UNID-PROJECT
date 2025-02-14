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
    Task<User?> Create(CreateUserDto createUserDto);
    Task<User> GetByUserNameThrowsNotFound(string userName);
    Task<User> DeactivateUserAsync(Guid id);
    Task<User> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
}

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenService _tokenService;
    private readonly IGenericServie<User> _genericService;
    private readonly ILogger<UsersService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid AuthenticatedUserId => Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
    
    public UsersService(IUsersRepository usersRepository, IGenericServie<User> genericService,
        ITokenService tokenService, ILogger<UsersService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _usersRepository = usersRepository;
        _genericService = genericService;
        _tokenService = tokenService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested ChangePassword", AuthenticatedUserId);
        
        // Get the user
        User user = await this.GetByIdThrowsNotFoundAsync(userId);
        
        if (user.IsActive == false)
        {
            _logger.LogInformation("User with Id {AuthenticatedUserId} requested ChangePassword but the user is already deactivated", AuthenticatedUserId);
            throw new BadRequestException(message: "The user is already deactivated.", field: "IsActive");
        }
        
        // Check password
        if (PasswordHelper.VerifyPassword(changePasswordDto.ActualPassword, user.UserPassword) == false)
        {
            _logger.LogInformation("User with Id {AuthenticatedUserId} requested ChangePassword but the actual password is not correct", AuthenticatedUserId);
            throw new UnauthorizedException(message: "The actual password is not correct.", reason: "WrongPassword");
        }
        
        // Change password
        user.UserPassword = PasswordHelper.HashPassword(changePasswordDto.NewPassword);
        
        // Save changes
        await this._usersRepository.SaveChangesAsync();
        
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested ChangePassword and the password was changed", AuthenticatedUserId);
        
        return user;
    }

    public async Task<User> DeactivateUserAsync(Guid id)
    {
        using var transaction = await _usersRepository.BeginTransactionAsync();

        try
        {
            _logger.LogInformation("User with Id {AuthenticatedUserId} requested DeactivateUser for UserId {TargetUserId}", AuthenticatedUserId, id);
            
            // Deactivate user
            User user = await this.GetByIdThrowsNotFoundAsync(id);
            
            if (user.IsActive == false)
            {
                _logger.LogInformation("User with Id {AuthenticatedUserId} requested DeactivateUser for UserId {TargetUserId} but the user is already deactivated", AuthenticatedUserId, id);
                throw new BadRequestException(message: "The user is already deactivated.", field: "IsActive");
            }
            
            user.IsActive = false;
        
            // Inactivate resfresh tokens
            await _tokenService.RevokeRefreshsTokensByUserId(user.UserId);
        
            // Save changes
            await this._usersRepository.SaveChangesAsync();
        
            await transaction.CommitAsync();
            
            _logger.LogInformation("User with Id {AuthenticatedUserId} requested DeactivateUser for UserId {TargetUserId} and the user was deactivated", AuthenticatedUserId, id);
            
            return user;
        } 
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
        
    }

    public async Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto, query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
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
        return await _genericService.GetFirstAsync(u => u.UserUserName, userName, query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User> GetByUserNameThrowsNotFound(string userName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(u => u.UserUserName, userName, query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _genericService.GetFirstAsync(u => u.UserEmail, email);
    }

    public async Task<User?> Create(CreateUserDto createUserDto)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested CreateUser with UserName {UserUserName}", AuthenticatedUserId, createUserDto.UserUserName);
        if (await this.GetByUserName(createUserDto.UserUserName) != null)
        {
            _logger.LogError("User with Id {AuthenticatedUserId} requested CreateUser but the user with username {UserUserName} already exists", AuthenticatedUserId, createUserDto.UserUserName);
            throw new UniqueConstraintViolationException(
                message: $"User with username {createUserDto.UserUserName} already exists", field: "UserUserName");
        }

        if (await this.GetByEmail(createUserDto.UserEmail) != null)
        {
            _logger.LogError("User with Id {AuthenticatedUserId} requested CreateUser but the user with email {UserEmail} already exists", AuthenticatedUserId, createUserDto.UserEmail);
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

        _logger.LogInformation("User with Id {AuthenticatedUserId} requested CreateUser and the user was created with Id {CreatedUserId}", AuthenticatedUserId, user.UserId);
        
        return user;
    }
}