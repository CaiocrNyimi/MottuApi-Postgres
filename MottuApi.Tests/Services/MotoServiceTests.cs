using Xunit;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Services.Implementations;
using MottuApi.Dtos;

namespace MottuApi.Tests.Services
{
    public class MotoServiceTests
    {
        [Fact]
        public async Task CreateAsync_DeveCriarMoto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("MotoDb")
                .Options;

            using var context = new AppDbContext(options);
            var service = new MotoService(context);

            var dto = new MotoRequestDto
            {
                Placa = "ABC1D23",
                Modelo = "CG 160",
                Status = "Disponível",
                PatioId = null,
                DataEntrada = null
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Placa, result.Placa);
        }
    }
}