using System.Security.Claims;

namespace CRM_ERP_UNID.Controllers;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims); // Genera el token de acceso (JWT)
    string GenerateRefreshToken(); // Genera un token de actualización (refresh token)
}