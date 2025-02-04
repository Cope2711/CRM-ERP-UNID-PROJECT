using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;
[Table("Permissions")]
    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string PermissionName { get; set; }

        public required string Description { get; set; } 
    }

