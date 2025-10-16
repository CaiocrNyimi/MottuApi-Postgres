using MottuApi.Data;
using MottuApi.Models;
using MottuApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MottuApi.Services.Implementations
{
    public class MovimentacaoService : IMovimentacaoService
    {
        private readonly AppDbContext _context;

        public MovimentacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movimentacao>> GetAllAsync()
        {
            return await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.Patio)
                .ToListAsync();
        }

        public async Task<Movimentacao?> GetByIdAsync(int id)
        {
            return await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movimentacao> CreateAsync(Movimentacao movimentacao)
        {
            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();
            return movimentacao;
        }

        public async Task<bool> UpdateAsync(int id, Movimentacao movimentacao)
        {
            var existing = await _context.Movimentacoes.FindAsync(id);
            if (existing == null) return false;

            existing.MotoId = movimentacao.MotoId;
            existing.PatioId = movimentacao.PatioId;
            existing.DataEntrada = movimentacao.DataEntrada;
            existing.DataSaida = movimentacao.DataSaida;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);
            if (movimentacao == null) return false;

            _context.Movimentacoes.Remove(movimentacao);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}