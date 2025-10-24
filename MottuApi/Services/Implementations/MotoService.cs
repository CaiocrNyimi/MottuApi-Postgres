using MottuApi.Data;
using MottuApi.Dtos;
using MottuApi.Models;
using MottuApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MottuApi.Services.Implementations
{
    public class MotoService : IMotoService
    {
        private readonly AppDbContext _context;

        public MotoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MotoResponseDto>> GetAllAsync(int page, int size)
        {
            var motos = await _context.Motos
                .Include(m => m.Patio)
                .OrderBy(m => m.Id)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return motos.Select(m => new MotoResponseDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo,
                Status = m.Status,
                DataEntrada = m.DataEntrada,
                Patio = m.Patio == null? null: new PatioSimplificadoDto
                {
                    Id = m.Patio.Id,
                    Nome = m.Patio.Nome
                }
            });
        }

        public async Task<MotoResponseDto?> GetByIdAsync(int id)
        {
            var m = await _context.Motos
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (m == null) return null;

            return new MotoResponseDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo,
                Status = m.Status,
                DataEntrada = m.DataEntrada,
                Patio = m.Patio == null? null: new PatioSimplificadoDto
                {
                    Id = m.Patio.Id,
                    Nome = m.Patio.Nome
                }
            };
        }

        public async Task<MotoResponseDto> CreateAsync(MotoRequestDto dto)
        {

            var motoPlaca = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == dto.Placa);
            if (motoPlaca != null)
                throw new Exception("Já existe uma moto com essa placa.");

            var regex = new Regex(@"^[A-Z]{3}[0-9]{1}[A-Z]{1}[0-9]{2}$");
            if (!regex.IsMatch(dto.Placa))
                throw new Exception("Placa inválida. Use o padrão Mercosul (ABC1D23).");

            var moto = new Moto
            {
                Placa = dto.Placa,
                Modelo = dto.Modelo,
                Status = dto.Status,
                PatioId = dto.PatioId,
                DataEntrada = dto.PatioId != null ? dto.DataEntrada : null
            };

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            var patio = await _context.Patios.FindAsync(dto.PatioId);

            return new MotoResponseDto
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Modelo = moto.Modelo,
                Status = moto.Status,
                DataEntrada = moto.DataEntrada,
                Patio = new PatioSimplificadoDto
                {
                    Id = patio?.Id ?? 0,
                    Nome = patio?.Nome ?? ""
                }
            };
        }

        public async Task<bool> UpdateAsync(int id, MotoRequestDto dto)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return false;

            moto.Placa = dto.Placa;
            moto.Modelo = dto.Modelo;
            moto.Status = dto.Status;
            moto.PatioId = dto.PatioId;
            moto.DataEntrada = dto.PatioId != null ? dto.DataEntrada : null;

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