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
    public string RefreshToken { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
}