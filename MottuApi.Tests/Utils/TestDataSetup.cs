using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MottuApi.Tests.Utils
{
    public static class TestDataSetup
    {
        public static async Task SeedAsync(AppDbContext context)
        {

            context.Usuarios.RemoveRange(context.Usuarios);
            await context.SaveChangesAsync();

            context.Usuarios.Add(new Usuario
            {
                Username = "admin",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword("12345")
            });

            await context.SaveChangesAsync();
        }

        public static async Task<int> CriarPatioERetornarIdAsync(AppDbContext context)
        {
            var patio = new Patio
            {
                Nome = $"Patio Teste {Guid.NewGuid()}",
                Localizacao = "Rua Teste, 123"
            };

            context.Patios.Add(patio);
            await context.SaveChangesAsync();
            return patio.Id;
        }

        public static async Task<int> CriarMotoSemPatioERetornarIdAsync(AppDbContext context)
        {
            var moto = new Moto
            {
                Placa = $"ABC{new Random().Next(1000, 9999)}",
                Modelo = "Honda CG",
                Status = "Disponível"
            };

            context.Motos.Add(moto);
            await context.SaveChangesAsync();
            return moto.Id;
        }

        public static async Task RemoverMotoAsync(AppDbContext context, int motoId)
        {
            var moto = await context.Motos.FindAsync(motoId);
            if (moto != null)
            {
                context.Motos.Remove(moto);
                await context.SaveChangesAsync();
            }
        }

        public static async Task RemoverPatioAsync(AppDbContext context, int patioId)
        {
            var patio = await context.Patios.FindAsync(patioId);
            if (patio != null)
            {
                context.Patios.Remove(patio);
                await context.SaveChangesAsync();
            }
        }

        public static string GerarJwtDeTeste(AppDbContext context, string username = "admin")
        {
            var usuario = context.Usuarios.FirstOrDefault(u => u.Username == username)
                ?? throw new InvalidOperationException("Usuário não encontrado");
        
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim("UsuarioId", usuario.Id.ToString())
            };
        
            var key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY não definida");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "mottuapi";
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "mottuapi-users";
        
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
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