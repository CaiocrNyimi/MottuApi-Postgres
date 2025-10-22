using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MottuApi.Dtos;
using MottuApi.Services.Interfaces;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de pátios.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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
        public async Task<ActionResult<IEnumerable<PatioResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retorna um pátio pelo seu identificador.
        /// </summary>
        /// <param name="id">Id do pátio.</param>
        /// <returns>Pátio encontrado ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PatioResponseDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="dto">Dados do pátio.</param>
        /// <returns>Pátio criado.</returns>
        [HttpPost]
        public async Task<ActionResult<PatioResponseDto>> Create([FromBody] PatioRequestDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza um pátio existente.
        /// </summary>
        /// <param name="id">Id do pátio.</param>
        /// <param name="dto">Dados atualizados.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatioRequestDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
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
            return success ? NoContent() : NotFound();
        }
    }
}