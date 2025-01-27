using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Data.Models;

    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string PermissionName { get; set; }

        public string Description { get; set; } 
    }

