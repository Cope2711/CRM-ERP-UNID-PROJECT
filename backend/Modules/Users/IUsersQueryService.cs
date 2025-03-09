using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersQueryService
{
    Task<GetAllResponseDto<User>> GetAll(GetAllDto getAllDto);
    Task<User> GetByIdThrowsNotFoundAsync(Guid id);
    Task<User?> GetById(Guid id);
    Task<User?> GetByUserName(string userName);
    Task<User> GetByUserNameThrowsNotFound(string userName);
    Task<User> GetByEmailThrowsNotFoundAsync(string email);
    Task<bool> ExistById(Guid id);
    Task<bool> ExistByEmail(string email);
    Task<bool> ExistByUserName(string userName);
}