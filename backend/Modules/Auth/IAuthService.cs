using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IAuthService
{
    Task<TokenDto> Login(LoginUserDto loginUserDto);
    Task<TokenDto> RefreshTokenAsync(RefreshTokenEntryDto refreshTokenEntryDto);
    Task<RefreshToken?> Logout(string refreshTokenString);
    
}