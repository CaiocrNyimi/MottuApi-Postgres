using MottuApi.Data;
using MottuApi.Models;
using MottuApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MottuApi.Services.Implementations
{
    public class MotoService : IMotoService
    {
        private readonly AppDbContext _context;

        public MotoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Moto>> GetAllAsync(int page, int size)
        {
            return await _context.Motos
                .Include(m => m.Patio)
                .OrderBy(m => m.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<Moto?> GetByIdAsync(int id)
        {
            return await _context.Motos
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Moto> CreateAsync(Moto moto)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return moto;
        }

        public async Task<bool> UpdateAsync(int id, Moto moto)
        {
            var existing = await _context.Motos.FindAsync(id);
            if (existing == null) return false;

            existing.Placa = moto.Placa;
            existing.Modelo = moto.Modelo;
            existing.Status = moto.Status;
            existing.PatioId = moto.PatioId;
            existing.DataEntrada = moto.DataEntrada;
            existing.DataSaida = moto.DataSaida;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return false;

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}