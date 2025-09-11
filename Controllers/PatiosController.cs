using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Models;
using System.Threading.Tasks;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de pátios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PatiosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatiosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna uma lista paginada de pátios.
        /// </summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10).</param>
        /// <returns>Lista paginada de pátios com links HATEOAS.</returns>
        [HttpGet]
        public async Task<ActionResult<object>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var total = await _context.Patios.CountAsync();
            var patios = await _context.Patios
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = patios.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Localizacao,
                links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id = p.Id }) },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = p.Id }) },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = p.Id }) }
                }
            });

            return Ok(new { total, page, pageSize, items });
        }

        /// <summary>
        /// Retorna um pátio pelo seu identificador.
        /// </summary>
        /// <param name="id">Id do pátio.</param>
        /// <returns>Pátio encontrado ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Patio>> GetById(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null) return NotFound();
            return Ok(patio);
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="patio">Dados do pátio.</param>
        /// <returns>Pátio criado.</returns>
        [HttpPost]
        public async Task<ActionResult<Patio>> Create(Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = patio.Id }, patio);
        }

        /// <summary>
        /// Atualiza um pátio existente.
        /// </summary>
        /// <param name="id">Id do pátio.</param>
        /// <param name="patio">Dados atualizados.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Patio patio)
        {
            if (id != patio.Id) return BadRequest("IDs diferentes.");

            var existente = await _context.Patios.FindAsync(id);
            if (existente == null) return NotFound();

            existente.Nome = patio.Nome;
            existente.Localizacao = patio.Localizacao;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Exclui um pátio pelo id.
        /// </summary>
        /// <param name="id">Id do pátio.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null) return NotFound();

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}