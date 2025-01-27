using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Dtos;



public class AssignPermissionDtos
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}