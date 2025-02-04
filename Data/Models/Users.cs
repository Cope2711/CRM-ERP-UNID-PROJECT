using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_ERP_UNID.Data.Models;

[Table("Users")]
public class User
{
    [Key]
    public Guid UserId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UserUserName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UserFirstName { get; set; }
    
    [Required]
    [MaxLength(50)]

    public string UserLastName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string UserEmail { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string UserPassword { get; set; }
    
    [Required]
    public bool IsActive { get; set; }

    //relacion co Role
    public Guid RoleId { get; set; }

    [ForeignKey("RoleId")]
    public Role Role { get; set; }
}