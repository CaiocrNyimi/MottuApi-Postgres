using MottuApi.Data;
using MottuApi.Dtos;
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

        public async Task<IEnumerable<MovimentacaoResponseDto>> GetAllAsync()
        {
            var movimentacoes = await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.Patio)
                .ToListAsync();

            return movimentacoes.Select(m => new MovimentacaoResponseDto
            {
                Id = m.Id,
                DataEntrada = m.DataEntrada,
                DataSaida = m.DataSaida,
                Moto = new MotoSimplificadaDto
                {
                    Id = m.Moto.Id,
                    Placa = m.Moto.Placa
                },
                Patio = new PatioSimplificadoDto
                {
                    Id = m.Patio.Id,
                    Nome = m.Patio.Nome
                }
            });
        }

        public async Task<MovimentacaoResponseDto?> GetByIdAsync(int id)
        {
            var m = await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (m == null) return null;

            return new MovimentacaoResponseDto
            {
                Id = m.Id,
                DataEntrada = m.DataEntrada,
                DataSaida = m.DataSaida,
                Moto = new MotoSimplificadaDto
                {
                    Id = m.Moto.Id,
                    Placa = m.Moto.Placa
                },
                Patio = new PatioSimplificadoDto
                {
                    Id = m.Patio.Id,
                    Nome = m.Patio.Nome
                }
            };
        }

        public async Task<MovimentacaoResponseDto> CreateAsync(MovimentacaoRequestDto dto)
        {
            var movimentacao = new Movimentacao
            {
                MotoId = dto.MotoId,
                PatioId = dto.PatioId,
                DataEntrada = dto.DataEntrada,
                DataSaida = dto.DataSaida
            };

            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();

            var moto = await _context.Motos.FindAsync(dto.MotoId);
            if (moto != null)
            {
                moto.PatioId = dto.PatioId;
                moto.DataEntrada = dto.DataEntrada;
                await _context.SaveChangesAsync();
            }
            
            var motoInfo = await _context.Motos.FindAsync(dto.MotoId);
            var patioInfo = await _context.Patios.FindAsync(dto.PatioId);

            return new MovimentacaoResponseDto
            {
                Id = movimentacao.Id,
                DataEntrada = movimentacao.DataEntrada,
                DataSaida = movimentacao.DataSaida,
                Moto = new MotoSimplificadaDto
                {
                    Id = motoInfo?.Id ?? 0,
                    Placa = motoInfo?.Placa ?? ""
                },
                Patio = new PatioSimplificadoDto
                {
                    Id = patioInfo?.Id ?? 0,
                    Nome = patioInfo?.Nome ?? ""
                }
            };
        }

        public async Task<bool> UpdateAsync(int id, MovimentacaoRequestDto dto)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);
            if (movimentacao == null) return false;

            movimentacao.MotoId = dto.MotoId;
            movimentacao.PatioId = dto.PatioId;
            movimentacao.DataEntrada = dto.DataEntrada;
            movimentacao.DataSaida = dto.DataSaida;
            
            if (dto.DataSaida != null)
            {
                var moto = await _context.Motos.FindAsync(dto.MotoId);
                if (moto != null)
                {
                    moto.PatioId = null;
                    moto.DataEntrada = null;
                    await _context.SaveChangesAsync();
            }
            }
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