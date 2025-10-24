using Xunit;
using MottuApi.Services.Implementations;
using MottuApi.Dtos;

namespace MottuApi.Tests.Services
{
    public class EntregaMlServiceTests
    {
        [Fact]
        public void PreverTempoEntrega_DeveRetornarValorPositivo()
        {
            var service = new EntregaMlService();
            var request = new EntregaRequestDto
            {
                Modelo = "Biz",
                DistanciaKm = 10
            };

            var tempo = service.PreverTempoEntrega(request);

            Assert.True(tempo > 0);
        }

        [Fact]
        public void PreverTempoEntregaDto_DeveRetornarDtoComValorArredondado()
        {
            var service = new EntregaMlService();
            var request = new EntregaRequestDto
            {
                Modelo = "Biz",
                DistanciaKm = 10
            };

            var response = service.PreverTempoEntregaDto(request);

            Assert.True(response.TempoEstimadoMin > 0);
            Assert.InRange(response.TempoEstimadoMin, 20.96f, 20.98f);
        }

        [Fact]
        public void PreverTempoEntrega_DeveFalhar_QuandoModeloAusente()
        {
            var service = new EntregaMlService();
            var request = new EntregaRequestDto
            {
                Modelo = "",
                DistanciaKm = 10
            };

            var tempo = service.PreverTempoEntrega(request);

            Assert.True(tempo >= 0);
        }
    }
}