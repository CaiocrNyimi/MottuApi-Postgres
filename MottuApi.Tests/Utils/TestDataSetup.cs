using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MottuApi.Tests.Utils
{
    public static class TestDataSetup
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Usuarios.Any())
            {
                context.Usuarios.Add(new Usuario
                {
                    Username = "admin",
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword("12345")
                });

                await context.SaveChangesAsync();
            }
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
    }
}