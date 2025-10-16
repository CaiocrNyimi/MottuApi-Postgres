using Microsoft.AspNetCore.Mvc;
using MottuApi.Models;
using MottuApi.Services.Interfaces;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de pátios.
    /// </summary>
    [ApiController]
    [Route("api/patios")]
    public class PatioController : ControllerBase
    {
        private readonly IPatioService _service;

        public PatioController(IPatioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todos os pátios.
        /// </summary>
        /// <returns>Lista de pátios.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patios = await _service.GetAllAsync();
            return Ok(patios);
        }

        /// <summary>
        /// Retorna um pátio pelo seu identificador.
        /// </summary>
        /// <param name="id">Id do pátio.</param>
        /// <returns>Pátio encontrado ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patio = await _service.GetByIdAsync(id);
            if (patio == null) return NotFound();
            return Ok(patio);
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="patio">Dados do pátio.</param>
        /// <returns>Pátio criado.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Patio patio)
        {
            var created = await _service.CreateAsync(patio);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
            var success = await _service.UpdateAsync(id, patio);
            if (!success) return NotFound();
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
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}