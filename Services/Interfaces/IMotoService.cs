using MottuApi.Models;

namespace MottuApi.Services.Interfaces
{
    public interface IMotoService
    {
        Task<IEnumerable<Moto>> GetAllAsync(int page, int size);
        Task<Moto?> GetByIdAsync(int id);
        Task<Moto> CreateAsync(Moto moto);
        Task<bool> UpdateAsync(int id, Moto moto);
        Task<bool> DeleteAsync(int id);
    }
}