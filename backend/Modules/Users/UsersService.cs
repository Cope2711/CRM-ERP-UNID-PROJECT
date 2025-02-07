using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public interface IUsersService
{
    Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto);
    Task<User> GetByIdThrowsNotFoundAsync(Guid id);
    Task<User?> GetByUserName(string userName);
    Task<User?> GetByEmail(string email);
    Task<User?> Create(CreateUserDto createUserDto);
    Task<User> GetByUserNameThrowsNotFound(string userName);
}

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IGenericServie<User> _genericService;

    public UsersService(IUsersRepository usersRepository, IGenericServie<User> genericService)
    {
        _usersRepository = usersRepository;
        _genericService = genericService;
    }

    public async Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto, u => u.Role);
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _genericService.GetById(id, u => u.Role);
    }

    public async Task<User> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id, u => u.Role);
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await _genericService.GetFirstAsync(u => u.UserUserName, userName);
    }

    public async Task<User> GetByUserNameThrowsNotFound(string userName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(u => u.UserUserName, userName);
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