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
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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

            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}