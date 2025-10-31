using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MottuApi.Dtos;
using MottuApi.Services.Interfaces;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de movimentações.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMovimentacaoService _service;

        public MovimentacaoController(IMovimentacaoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todas as movimentações.
        /// </summary>
        /// <returns>Lista de movimentações.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovimentacaoResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Retorna uma movimentação pelo seu identificador.
        /// </summary>
        /// <param name="id">Id da movimentação.</param>
        /// <returns>Movimentação encontrada ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentacaoResponseDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Cria uma nova movimentação.
        /// </summary>
        /// <param name="dto">Dados da movimentação.</param>
        /// <returns>Movimentação criada.</returns>
        [HttpPost]
        public async Task<ActionResult<MovimentacaoResponseDto>> Create([FromBody] MovimentacaoRequestDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza uma movimentação existente.
        /// </summary>
        /// <param name="id">Id da movimentação.</param>
        /// <param name="dto">Dados atualizados.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovimentacaoRequestDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        /// <summary>
        /// Exclui uma movimentação pelo id.
        /// </summary>
        /// <param name="id">Id da movimentação.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}