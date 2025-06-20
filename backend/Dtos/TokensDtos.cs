using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Dtos;

public class TokenDto
{
    [Required]
    public string? token { get; set; }
    
    [Required]
    public string? refreshToken { get; set; }
    
    public UserDto? User { get; set; }
}

public class RefreshTokenEntryDto
{
    [Required]
    public required string RefreshToken { get; set; }
    
    [Required]
    public required string DeviceId { get; set; }
}