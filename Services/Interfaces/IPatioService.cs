using MottuApi.Models;

namespace MottuApi.Services.Interfaces
{
    public interface IPatioService
    {
        Task<IEnumerable<Patio>> GetAllAsync();
        Task<Patio?> GetByIdAsync(int id);
        Task<Patio> CreateAsync(Patio patio);
        Task<bool> UpdateAsync(int id, Patio patio);
        Task<bool> DeleteAsync(int id);
    }
}