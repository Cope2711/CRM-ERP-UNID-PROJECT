using System.Text.Json;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IUsersManagementService
{
    Task<User> Create(User data);
    Task<User> Update(Guid id, JsonElement data);
    Task<ResponsesDto<IdResponseStatusDto>> Activate(IdsDto idsDto);
    Task<ResponsesDto<IdResponseStatusDto>> Deactivate(IdsDto idsDto);
    Task<User> ChangePassword(ChangePasswordDto changePasswordDto);
}