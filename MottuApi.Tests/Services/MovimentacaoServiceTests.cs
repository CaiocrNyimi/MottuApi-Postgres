using Xunit;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Services.Implementations;
using MottuApi.Dtos;
using MottuApi.Models;

namespace MottuApi.Tests.Services
{
    public class MovimentacaoServiceTests
    {
        [Fact]
        public async Task CreateAsync_DeveCriarMovimentacao()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("MovimentacaoDb")
                .Options;

            using var context = new AppDbContext(options);

            var moto = new Moto { Placa = "ABC1D23", Modelo = "CG", Status = "Disponível" };
            var patio = new Patio { Nome = "Central", Localizacao = "Rua A" };
            context.Motos.Add(moto);
            context.Patios.Add(patio);
            await context.SaveChangesAsync();

            var service = new MovimentacaoService(context);

            var dto = new MovimentacaoRequestDto
            {
                MotoId = moto.Id,
                PatioId = patio.Id,
                DataEntrada = DateTime.Now
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("ABC1D23", result.Moto.Placa);
        }
    }
}