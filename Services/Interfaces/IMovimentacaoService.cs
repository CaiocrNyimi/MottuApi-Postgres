using MottuApi.Models;

namespace MottuApi.Services.Interfaces
{
    public interface IMovimentacaoService
    {
        Task<IEnumerable<Movimentacao>> GetAllAsync();
        Task<Movimentacao?> GetByIdAsync(int id);
        Task<Movimentacao> CreateAsync(Movimentacao movimentacao);
        Task<bool> UpdateAsync(int id, Movimentacao movimentacao);
        Task<bool> DeleteAsync(int id);
    }
}