﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;
[Table("Roles")]

    public class Role
    {
        [Key]
        public Guid RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string RoleName { get; set; }
        
        public ICollection<RolePermission>? RolePermissions { get; set; }
        
    }

