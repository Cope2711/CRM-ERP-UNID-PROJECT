using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Controllers;

public interface IUsersService
{
    Task<User?> GetById(Guid id);
    Task<User?> GetByUserName(string userName);
    Task<User?> GetByEmail(string email);
    Task<User?> Create(CreateUserDto createUserDto);
}

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        this._usersRepository = usersRepository;
    }

    public async Task<User?> GetById(Guid id)
    {
        return await this._usersRepository.GetById(id);
    }  

    public async Task<User?> GetByUserName(string userName)
    {
        return await this._usersRepository.GetByUserName(userName);
    }
    
    public async Task<User?> GetByEmail(string email)
    {
        return await this._usersRepository.GetByEmail(email);
    }

    public async Task<User?> Create(CreateUserDto createUserDto)
    {
        if (await this.GetByUserName(createUserDto.UserUserName) != null)
        {
            return null;
        }

        if (await this.GetByEmail(createUserDto.UserEmail) != null)
        {
            return null;
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