using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Models;
using System.Threading.Tasks;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para opera��es CRUD de movimenta��es.
    /// </summary>
    [ApiController]
    [Route("api/movimentacoes")]
    public class MovimentacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimentacoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista paginada de movimenta��es.
        /// </summary>
        /// <param name="page">N�mero da p�gina (padr�o: 1).</param>
        /// <param name="pageSize">Tamanho da p�gina (padr�o: 10).</param>
        /// <returns>Lista paginada de movimenta��es com links HATEOAS.</returns>
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
        /// Retorna uma movimenta��o pelo seu identificador.
        /// </summary>
        /// <param name="id">Id da movimenta��o.</param>
        /// <returns>Movimenta��o encontrada ou 404.</returns>
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
        /// Cria uma nova movimenta��o.
        /// </summary>
        /// <param name="movimentacao">Dados da movimenta��o.</param>
        /// <returns>Movimenta��o criada.</returns>
        [HttpPost]
        public async Task<ActionResult<Movimentacao>> Create(Movimentacao movimentacao)
        {
            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = movimentacao.Id }, movimentacao);
        }

        /// <summary>
        /// Atualiza uma movimenta��o existente.
        /// </summary>
        /// <param name="id">Id da movimenta��o.</param>
        /// <param name="movimentacao">Dados atualizados.</param>
        /// <returns>Status da opera��o.</returns>
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
        /// Exclui uma movimenta��o pelo id.
        /// </summary>
        /// <param name="id">Id da movimenta��o.</param>
        /// <returns>Status da opera��o.</returns>
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