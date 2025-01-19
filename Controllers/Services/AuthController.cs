using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM_ERP_UNID.Controllers.Services
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly ITokenService _authService;

        public AuthController(ITokenService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]

        public IActionResult Login([FromBody] LoginModel login)
        {
            if (login.Username == "user" && login.Password == "password") // Simulando validación
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Username),
                    new Claim(ClaimTypes.Role, "User")
                };

                var accessToken = _authService.GenerateAccessToken(claims);
                var refreshToken = _authService.GenerateRefreshToken();



                return Ok(new
                {
                    Accesstoken = accessToken,
                    RefreshToken = refreshToken

                });


            }
            // Respuesta inválidas de credenciales
            return Unauthorized("Usuario o contraseña incorrectos.");

        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}