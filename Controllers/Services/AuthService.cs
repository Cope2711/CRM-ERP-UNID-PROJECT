using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRM_ERP_UNID.Controllers.Services

{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpireMinutes"])),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }
    }
}


