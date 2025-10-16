using MottuApi.Data;
using MottuApi.Models;
using MottuApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MottuApi.Services.Implementations
{
    public class PatioService : IPatioService
    {
        private readonly AppDbContext _context;

        public PatioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patio>> GetAllAsync()
        {
            return await _context.Patios
                .Include(p => p.Motos)
                .Include(p => p.Movimentacoes)
                .ToListAsync();
        }

        public async Task<Patio?> GetByIdAsync(int id)
        {
            return await _context.Patios
                .Include(p => p.Motos)
                .Include(p => p.Movimentacoes)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Patio> CreateAsync(Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();
            return patio;
        }

        public async Task<bool> UpdateAsync(int id, Patio patio)
        {
            var existing = await _context.Patios.FindAsync(id);
            if (existing == null) return false;

            existing.Nome = patio.Nome;
            existing.Localizacao = patio.Localizacao;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null) return false;

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}