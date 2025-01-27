using CRM_ERP_UNID.Dtos;
using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Data.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
