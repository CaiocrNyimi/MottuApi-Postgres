using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MottuApi.Dtos;
using MottuApi.Services.Interfaces;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de motos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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
        public async Task<ActionResult<IEnumerable<MotoResponseDto>>> GetAll(int page = 1, int size = 10)
        {
            var result = await _service.GetAllAsync(page, size);
            return Ok(result);
        }

        /// <summary>
        /// Retorna uma moto pelo seu identificador.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        /// <returns>Moto encontrada ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MotoResponseDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="dto">Dados da moto.</param>
        /// <returns>Moto criada.</returns>
        [HttpPost]
        public async Task<ActionResult<MotoResponseDto>> Create([FromBody] MotoRequestDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        /// <param name="dto">Dados atualizados.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MotoRequestDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
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
            return success ? NoContent() : NotFound();
        }
    }
}