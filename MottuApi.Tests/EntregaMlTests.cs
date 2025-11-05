using Xunit;
using MottuApi.Services.Implementations;
using MottuApi.Dtos;

namespace MottuApi.Tests
{
    public class EntregaMlTests
    {
        [Fact]
        public void PreverEntrega_DeveRetornarTempoPositivo()
        {
            var service = new EntregaMlService();
            var request = new EntregaRequestDto
            {
                Modelo = "CG 160",
                DistanciaKm = 5
            };

            var resultado = service.PreverTempoEntregaDto(request);

            Assert.True(resultado.TempoEstimadoMin > 0);
        }
    }
}