using MottuApi.Data;
using MottuApi.Dtos;
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

        public async Task<IEnumerable<PatioResponseDto>> GetAllAsync()
        {
            var patios = await _context.Patios
                .Include(p => p.Motos)
                .Include(p => p.Movimentacoes)
                    .ThenInclude(m => m.Moto)
                .ToListAsync();

            return patios.Select(p => new PatioResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Localizacao = p.Localizacao,
                Motos = p.Motos.Select(m => new MotoSimplificadaDto
                {
                    Id = m.Id,
                    Placa = m.Placa
                }).ToList(),
                Movimentacoes = p.Movimentacoes.Select(m => new MovimentacaoSimplificadaDto
                {
                    Id = m.Id,
                    DataEntrada = m.DataEntrada,
                    DataSaida = m.DataSaida,
                    Moto = m.Moto == null ? null : new MotoSimplificadaDto
                    {
                        Id = m.Moto.Id,
                        Placa = m.Moto.Placa
                    }
                }).ToList()
            });
        }

        public async Task<PatioResponseDto?> GetByIdAsync(int id)
        {
            var p = await _context.Patios
                .Include(p => p.Motos)
                .Include(p => p.Movimentacoes)
                    .ThenInclude( m => m.Moto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null) return null;

            return new PatioResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Localizacao = p.Localizacao,
                Motos = p.Motos.Select(m => new MotoSimplificadaDto
                {
                    Id = m.Id,
                    Placa = m.Placa
                }).ToList(),
                Movimentacoes = p.Movimentacoes.Select(m => new MovimentacaoSimplificadaDto
                {
                    Id = m.Id,
                    DataEntrada = m.DataEntrada,
                    DataSaida = m.DataSaida,
                    Moto = m.Moto == null ? null : new MotoSimplificadaDto
                    {
                        Id = m.Moto.Id,
                        Placa = m.Moto.Placa
                    }
                }).ToList()
            };
        }

        public async Task<PatioResponseDto> CreateAsync(PatioRequestDto dto)
        {
            
            var patioNome = await _context.Patios.FirstOrDefaultAsync(p => p.Nome == dto.Nome);
            if (patioNome != null)
                throw new Exception("Já existe um pátio com esse nome.");
            
            var patioEndereco = await _context.Patios.FirstOrDefaultAsync(p => p.Localizacao == dto.Localizacao);
            if (patioEndereco != null)
                throw new Exception("Já existe um pátio com esse endereço.");
            
            var patio =
             new Patio
            {
                Nome = dto.Nome,
                Localizacao = dto.Localizacao
            };

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            return new PatioResponseDto
            {
                Id = patio.Id,
                Nome = patio.Nome,
                Localizacao = patio.Localizacao,
                Motos = new(),
                Movimentacoes = new()
            };
        }

        public async Task<bool> UpdateAsync(int id, PatioRequestDto dto)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null) return false;

            patio.Nome = dto.Nome;
            patio.Localizacao = dto.Localizacao;

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