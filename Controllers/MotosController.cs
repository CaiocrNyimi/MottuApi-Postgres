using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Models;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de motos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista paginada de motos.
        /// </summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10).</param>
        /// <returns>Lista paginada de motos com links HATEOAS.</returns>
        [HttpGet]
        public async Task<ActionResult<object>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var total = await _context.Motos.CountAsync();
            var motos = await _context.Motos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = motos.Select(m => new
            {
                m.Id,
                m.Placa,
                m.Status,
                m.Patio,
                m.DataEntrada,
                m.DataSaida,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id = m.Id }) },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = m.Id }) },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = m.Id }) }
                }
            });

            return Ok(new { total, page, pageSize, items });
        }

        /// <summary>
        /// Retorna uma moto pelo seu identificador.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        /// <returns>Moto encontrada ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetById(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        /// <summary>
        /// Busca uma moto pela placa.
        /// </summary>
        /// <param name="placa">Placa da moto.</param>
        /// <returns>Moto encontrada ou 404.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<Moto>> SearchByPlaca([FromQuery] string placa)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="moto">Dados da moto.</param>
        /// <returns>Moto criada.</returns>
        [HttpPost]
        public async Task<ActionResult<Moto>> Create(Moto moto)
        {
            if (string.IsNullOrWhiteSpace(moto.Placa))
                return BadRequest("Placa obrigatória.");

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = moto.Id }, moto);
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        /// <param name="moto">Dados atualizados.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Moto moto)
        {
            if (id != moto.Id) return BadRequest("IDs diferentes.");

            var existente = await _context.Motos.FindAsync(id);
            if (existente == null) return NotFound();

            existente.Placa = moto.Placa;
            existente.Status = moto.Status;
            existente.Patio = moto.Patio;
            existente.DataEntrada = moto.DataEntrada;
            existente.DataSaida = moto.DataSaida;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Exclui uma moto pelo id.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}