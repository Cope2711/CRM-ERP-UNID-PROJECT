using System.Text.Json;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface IRolesManagementService
{
    Task<Role> Create(Role data);
    Task<Role> Update(Guid id, JsonElement data);
    Task<Role> DeleteById(Guid id);
}