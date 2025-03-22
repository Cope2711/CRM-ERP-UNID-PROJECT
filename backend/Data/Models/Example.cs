using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

public class Example
{
    [Key]
    public Guid ExampleId { get; set; }
    
    [MaxLength(50)]
    public required string ExampleName { get; set; }
}

public static class ExampleExtensions
{
    public static ExampleDto ToDto(this Example example)
    {
        return new ExampleDto
        {
            ExampleId = example.ExampleId,
            ExampleName = example.ExampleName
        };
    }
}