using MottuApi.Dtos;

namespace MottuApi.Services.Interfaces
{
    public interface IEntregaMlService
    {
        float PreverTempoEntrega(EntregaRequestDto request);
        EntregaResponseDto PreverTempoEntregaDto(EntregaRequestDto request);
    }
}