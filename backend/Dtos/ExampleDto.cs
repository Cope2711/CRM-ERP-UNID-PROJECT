using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class ExampleDto
{
    [Required]
    [GuidNotEmpty]
    public Guid ExampleId { get; set; }
    
    [MaxLength(50)]
    public required string ExampleName { get; set; }
}

public class CreateExampleDto
{
    [Required]
    [MaxLength(50)]
    public required string ExampleName { get; set; }
}

public class UpdateExampleDto
{
    [Required]
    [GuidNotEmpty]
    public Guid ExampleId { get; set; }
    
    [MaxLength(50)]
    public string? ExampleName { get; set; }
}