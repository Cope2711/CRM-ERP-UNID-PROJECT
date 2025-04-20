using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class BranchDto
{
    [GuidNotEmpty]
    public Guid BranchId { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string BranchName { get; set; }
    
    [Required]
    [StringLength(255)]
    public required string BranchAddress { get; set; }
    
    [Required]
    [StringLength(20)]
    public string? BranchPhone { get; set; }
    
    public required bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public class CreateBranchDto
{
    [Required]
    [StringLength(100)]
    public required string BranchName { get; set; }
    
    [Required]
    [StringLength(255)]
    public required string BranchAddress { get; set; }
    
    [Required]
    [StringLength(20)]
    public required string BranchPhone { get; set; }

    [Required] 
    public bool IsActive { get; set; } = true;
}

public class UpdateBranchDto
{
    [MinLength(4)]
    [MaxLength(255)]
    public string? BranchName { get; set; }
    
    [MinLength(4)]
    [MaxLength(255)]
    public string? BranchAddress { get; set; }
    
    public string? BranchPhone { get; set; }
    public bool IsActive { get; set; }
}