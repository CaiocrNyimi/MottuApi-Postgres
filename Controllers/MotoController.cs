using Microsoft.AspNetCore.Mvc;
using MottuApi.Models;
using MottuApi.Services.Interfaces;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de motos.
    /// </summary>
    [ApiController]
    [Route("api/motos")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _service;

        public MotoController(IMotoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna uma lista paginada de motos.
        /// </summary>
        /// <param name="page">Número da página (padrão: 1).</param>
        /// <param name="size">Tamanho da página (padrão: 10).</param>
        /// <returns>Lista paginada de motos.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int size = 10)
        {
            var motos = await _service.GetAllAsync(page, size);
            return Ok(motos);
        }

        /// <summary>
        /// Retorna uma moto pelo seu identificador.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        /// <returns>Moto encontrada ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var moto = await _service.GetByIdAsync(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="moto">Dados da moto.</param>
        /// <returns>Moto criada.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Moto moto)
        {
            var created = await _service.CreateAsync(moto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
            var success = await _service.UpdateAsync(id, moto);
            if (!success) return NotFound();
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
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}