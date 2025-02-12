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
}

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenService _tokenService;
    private readonly IGenericServie<User> _genericService;

    public UsersService(IUsersRepository usersRepository, IGenericServie<User> genericService, ITokenService tokenService)
    {
        _usersRepository = usersRepository;
        _genericService = genericService;
        _tokenService = tokenService;
    }
    
    public async Task<User> DeactivateUserAsync(Guid id)
    {
        using var transaction = await _usersRepository.BeginTransactionAsync();

        try
        {
            // Deactivate user
            User user = await this.GetByIdThrowsNotFoundAsync(id);
            
            if (user.IsActive == false)
            {
                throw new BadRequestException(message: "The user is already deactivated.", field: "IsActive");
            }
            
            user.IsActive = false;
        
            // Inactivate resfresh tokens
            await _tokenService.RevokeRefreshsTokensByUserId(user.UserId);
        
            // Save changes
            await this._usersRepository.SaveChangesAsync();
        
            await transaction.CommitAsync();
            
            return user;
        } 
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
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
        if (await this.GetByUserName(createUserDto.UserUserName) != null)
        {
            throw new UniqueConstraintViolationException(
                message: $"User with username {createUserDto.UserUserName} already exists", field: "UserUserName");
        }

        if (await this.GetByEmail(createUserDto.UserEmail) != null)
        {
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

        return user;
    }
}