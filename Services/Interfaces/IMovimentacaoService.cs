using MottuApi.Dtos;
using MottuApi.Models;

namespace MottuApi.Services.Interfaces
{
    public interface IMovimentacaoService
    {
        Task<IEnumerable<MovimentacaoResponseDto>> GetAllAsync();
        Task<MovimentacaoResponseDto?> GetByIdAsync(int id);
        Task<MovimentacaoResponseDto> CreateAsync(MovimentacaoRequestDto dto);
        Task<bool> UpdateAsync(int id, MovimentacaoRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}