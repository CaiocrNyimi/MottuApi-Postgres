using MottuApi.Dtos;

namespace MottuApi.Services.Interfaces
{
    public interface IMotoService
    {
        Task<IEnumerable<MotoResponseDto>> GetAllAsync(int page, int size);
        Task<MotoResponseDto?> GetByIdAsync(int id);
        Task<MotoResponseDto> CreateAsync(MotoRequestDto dto);
        Task<bool> UpdateAsync(int id, MotoRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}