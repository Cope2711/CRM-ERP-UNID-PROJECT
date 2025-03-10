using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class GetAllDto
{
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0.")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 999, ErrorMessage = "PageSize must be between 1 and 999.")]
    public int PageSize { get; set; } = 10;

    [MaxLength(250, ErrorMessage = "OrderBy cannot exceed 250 characters.")]
    public string? OrderBy { get; set; }

    public bool Descending { get; set; }

    public List<FilterDto>? Filters { get; set; }
    
    [Required(ErrorMessage = "Search is required.")]
    [RangeListLength(1, 100, ErrorMessage = "Selects cannot exceed 100 characters.")]
    public required List<string> Selects { get; set; }
}

public class FilterDto
{
    [Required(ErrorMessage = "Column is required.")]
    [MaxLength(250, ErrorMessage = "Column cannot exceed 250 characters.")]
    public string? Column { get; set; }
    
    [Required(ErrorMessage = "Operator is required.")]
    [ValidOperator]
    public string? Operator { get; set; }
    
    [Required(ErrorMessage = "Value is required.")]
    public string? Value { get; set; }
}

public class GetAllResponseDto<T>
{
    [Required(ErrorMessage = "Data is required.")]
    public List<Dictionary<string, object>> Data { get; set; } = new List<Dictionary<string, object>>();

    [Range(0, int.MaxValue, ErrorMessage = "TotalItems cannot be negative.")]
    public int TotalItems { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
    public int PageNumber { get; set; }

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "TotalPages must be at least 1.")]
    public int TotalPages { get; set; }
}

public class ResponsesDto<T>
{
    public List<T> Success { get; set; } = new();
    public List<T> Failed { get; set; } = new();
}

public class ResponseStatusDto
{
    public required string Status { get; set; }
    public string? Message { get; set; }
    public string? Field { get; set; }
}