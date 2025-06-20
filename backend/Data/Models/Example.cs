using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Data.Models;

public class Example
{
    [Key]
    public Guid id { get; set; }
    
    [MaxLength(50)]
    public string name { get; set; }
}

public static class ExampleExtensions
{
    public static ExampleDto ToDto(this Example example)
    {
        return new ExampleDto
        {
            id = example.id,
            name = example.name
        };
    }
}