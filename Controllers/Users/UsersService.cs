using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Controllers;

public interface IUsersService
{
    Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto);
    Task<User?> GetById(Guid id);
    Task<User> GetByIdThrowsNotFound(Guid id);
    Task<User?> GetByUserName(string userName);
    Task<User?> GetByEmail(string email);
    Task<User?> Create(CreateUserDto createUserDto);
    Task<User> GetByUserNameThrowsNotFound(string userName);
}

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        this._usersRepository = usersRepository;
    }

    public async Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto)
    {
        GetAllResponseDto<User> getAllResponseDto = new GetAllResponseDto<User>();
        getAllResponseDto.TotalItems = await this._usersRepository.GetTotalItems(getAllDto);
        getAllResponseDto.TotalPages = (int)Math.Ceiling((double)getAllResponseDto.TotalItems / getAllDto.PageSize);
        getAllResponseDto.PageNumber = getAllDto.PageNumber;
        getAllResponseDto.PageSize = getAllDto.PageSize;
        getAllResponseDto.Data = await this._usersRepository.GetAll(getAllDto);
        return getAllResponseDto;
    }

    public async Task<User?> GetById(Guid id)
    {
        return await this._usersRepository.GetById(id);
    }

    public async Task<User> GetByIdThrowsNotFound(Guid id)
    {
        User? user = await this._usersRepository.GetById(id);

        if (user == null)
            throw new NotFoundException(message: $"User with id: {id} not found!", field: "UserId");

        return user;
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await this._usersRepository.GetByUserName(userName);
    }

    public async Task<User> GetByUserNameThrowsNotFound(string userName)
    {
        User? user = await this._usersRepository.GetByUserName(userName);

        if (user == null)
            throw new NotFoundException(message: $"User with username: {userName} not found!", field: "UserUserName");

        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await this._usersRepository.GetByEmail(email);
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