using MottuApi.Dtos;

namespace MottuApi.Services.Interfaces
{
    public interface IPatioService
    {
        Task<IEnumerable<PatioResponseDto>> GetAllAsync();
        Task<PatioResponseDto?> GetByIdAsync(int id);
        Task<PatioResponseDto> CreateAsync(PatioRequestDto dto);
        Task<bool> UpdateAsync(int id, PatioRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}