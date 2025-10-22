using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MottuAPI.Services
{
    public static class TokenService
    {
        public static string GerarToken(string usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario)
            };

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sua-chave-super-secreta"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "MottuAPI",
                audience: "MottuAPIClient",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}