using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public class UsersQueryService(
    IGenericService<User> _genericService
) : IUsersQueryService
{
    public async Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto,
            query => query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
    }

    public async Task<User?> GetById(Guid id)
    {
        return await _genericService.GetById(id,
            query => query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.UsersBranches)
                .ThenInclude(ub => ub.Branch));
    }

    public async Task<User> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFound(id,
            query => query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.UsersBranches)
                .ThenInclude(ub => ub.Branch));
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await _genericService.GetFirstAsync(
            u => u.UserUserName, 
            userName,
            query => query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.UsersBranches)
                .ThenInclude(ub => ub.Branch)
        );
    }

    public async Task<User> GetByUserNameThrowsNotFound(string userName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(
            u => u.UserUserName, 
            userName,
            query => query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.UsersBranches)
                .ThenInclude(ub => ub.Branch)
        );
    }

    public async Task<bool> ExistByIdThrowsNotFound(Guid id)
    {
        if (!await ExistById(id))
            throw new NotFoundException(message: $"User with id {id} not exist", field: Fields.Users.UserId);
        return true;
    }

    public async Task<bool> ExistById(Guid id)
    {
        return await _genericService.ExistsAsync(u => u.UserId == id);
    }


    public async Task<bool> ExistByUserName(string userName)
    {
        return await _genericService.ExistsAsync(u => u.UserUserName == userName);
    }

    public async Task<bool> ExistByEmail(string email)
    {
        return await _genericService.ExistsAsync(u => u.UserEmail == email);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _genericService.GetFirstAsync(
            u => u.UserEmail, 
            email,
            query => query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.UsersBranches)
                .ThenInclude(ub => ub.Branch)
        );
    }

    public async Task<User> GetByEmailThrowsNotFoundAsync(string email)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(
            u => u.UserEmail, 
            email,
            query => query
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u => u.UsersBranches)
                .ThenInclude(ub => ub.Branch)
        );
    }
}