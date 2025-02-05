using System.ComponentModel.DataAnnotations;

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

    [MaxLength(250, ErrorMessage = "SearchTerm cannot exceed 250 characters.")]
    public string? SearchTerm { get; set; }

    [MaxLength(250, ErrorMessage = "SearchColumn cannot exceed 250 characters.")]
    public string? SearchColumn { get; set; }
}

public class GetAllResponseDto<T>
{
    [Required(ErrorMessage = "Data is required.")]
    public List<T> Data { get; set; } = new List<T>();

    [Range(0, int.MaxValue, ErrorMessage = "TotalItems cannot be negative.")]
    public int TotalItems { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1.")]
    public int PageNumber { get; set; }

    [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
    public int PageSize { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "TotalPages must be at least 1.")]
    public int TotalPages { get; set; }
}