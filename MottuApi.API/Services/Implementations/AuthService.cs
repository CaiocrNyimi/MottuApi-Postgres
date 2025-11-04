using MottuApi.Data;
using MottuApi.Dtos;
using MottuApi.Models;
using MottuApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MottuApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarAsync(RegisterRequestDto dto)
        {
            var existe = await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Username == dto.Username)
                .Select(u => 1)
                .FirstOrDefaultAsync() == 1;

            if (existe) return false;

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            var usuario = new Usuario
            {
                Username = dto.Username,
                SenhaHash = senhaHash
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> LoginAsync(LoginRequestDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            var rawKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            if (string.IsNullOrWhiteSpace(rawKey))
                throw new InvalidOperationException("JWT_KEY não definida");

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(rawKey));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}