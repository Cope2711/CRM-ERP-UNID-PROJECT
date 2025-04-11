using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Dtos;

public class TokenDto
{
    [Required]
    public string? Token { get; set; }
    
    [Required]
    public string? RefreshToken { get; set; }
}

public class RefreshTokenEntryDto
{
    [Required]
    public required string RefreshToken { get; set; }
    
    [Required]
    public required string DeviceId { get; set; }
}