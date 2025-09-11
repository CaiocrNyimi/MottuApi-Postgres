using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Models;
using System.Threading.Tasks;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de movimentações.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimentacoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista paginada de movimentações.
        /// </summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10).</param>
        /// <returns>Lista paginada de movimentações com links HATEOAS.</returns>
        [HttpGet]
        public async Task<ActionResult<object>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var total = await _context.Movimentacoes.CountAsync();
            var movimentacoes = await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.Patio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = movimentacoes.Select(m => new
            {
                m.Id,
                m.MotoId,
                m.PatioId,
                m.DataEntrada,
                m.DataSaida,
                Moto = m.Moto != null ? new { m.Moto.Id, m.Moto.Placa } : null,
                Patio = m.Patio != null ? new { m.Patio.Id, m.Patio.Nome } : null,
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
        /// Retorna uma movimentação pelo seu identificador.
        /// </summary>
        /// <param name="id">Id da movimentação.</param>
        /// <returns>Movimentação encontrada ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Movimentacao>> GetById(int id)
        {
            var movimentacao = await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.Patio)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movimentacao == null) return NotFound();
            return Ok(movimentacao);
        }

        /// <summary>
        /// Cria uma nova movimentação.
        /// </summary>
        /// <param name="movimentacao">Dados da movimentação.</param>
        /// <returns>Movimentação criada.</returns>
        [HttpPost]
        public async Task<ActionResult<Movimentacao>> Create(Movimentacao movimentacao)
        {
            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = movimentacao.Id }, movimentacao);
        }

        /// <summary>
        /// Atualiza uma movimentação existente.
        /// </summary>
        /// <param name="id">Id da movimentação.</param>
        /// <param name="movimentacao">Dados atualizados.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Movimentacao movimentacao)
        {
            if (id != movimentacao.Id) return BadRequest("IDs diferentes.");

            var existente = await _context.Movimentacoes.FindAsync(id);
            if (existente == null) return NotFound();

            existente.MotoId = movimentacao.MotoId;
            existente.PatioId = movimentacao.PatioId;
            existente.DataEntrada = movimentacao.DataEntrada;
            existente.DataSaida = movimentacao.DataSaida;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Exclui uma movimentação pelo id.
        /// </summary>
        /// <param name="id">Id da movimentação.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);
            if (movimentacao == null) return NotFound();

            _context.Movimentacoes.Remove(movimentacao);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}