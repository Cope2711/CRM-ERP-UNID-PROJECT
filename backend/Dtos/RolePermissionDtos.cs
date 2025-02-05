using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Dtos
{
    public class RolePermissionDto
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        
    }
}
