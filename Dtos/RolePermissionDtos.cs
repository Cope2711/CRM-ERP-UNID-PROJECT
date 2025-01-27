using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Dtos
{
    public class RolePermissionDtos
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid PermissionId { get; set; }
        
    }
}
