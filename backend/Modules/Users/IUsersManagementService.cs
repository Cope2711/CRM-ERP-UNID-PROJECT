using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersManagementService
{
    Task<User?> Create(CreateUserDto createUserDto);
    Task<User> Update(Guid id,UpdateUserDto updateUserDto);
    Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto);
    Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto);
    Task<User> ChangePassword(ChangePasswordDto changePasswordDto);
}