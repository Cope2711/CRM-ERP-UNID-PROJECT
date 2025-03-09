using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersManagementService
{
    Task<User?> Create(CreateUserDto createUserDto);
    Task<User> UpdateAsync(UpdateUserDto updateUserDto);
    Task<ResponsesDto<UserResponseStatusDto>> ActivateUsersAsync(UsersIdsDto usersIdsDto);
    Task<ResponsesDto<UserResponseStatusDto>> DeactivateUsersAsync(UsersIdsDto usersIdsDto);
    Task<User> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
}