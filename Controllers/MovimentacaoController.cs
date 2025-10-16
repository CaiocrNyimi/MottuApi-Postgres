using Microsoft.AspNetCore.Mvc;
using MottuApi.Models;
using MottuApi.Services.Interfaces;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller para operações CRUD de movimentações.
    /// </summary>
    [ApiController]
    [Route("api/movimentacoes")]
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
        public async Task<IActionResult> GetAll()
        {
            var movimentacoes = await _service.GetAllAsync();
            return Ok(movimentacoes);
        }

        /// <summary>
        /// Retorna uma movimentação pelo seu identificador.
        /// </summary>
        /// <param name="id">Id da movimentação.</param>
        /// <returns>Movimentação encontrada ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movimentacao = await _service.GetByIdAsync(id);
            if (movimentacao == null) return NotFound();
            return Ok(movimentacao);
        }

        /// <summary>
        /// Cria uma nova movimentação.
        /// </summary>
        /// <param name="movimentacao">Dados da movimentação.</param>
        /// <returns>Movimentação criada.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Movimentacao movimentacao)
        {
            var created = await _service.CreateAsync(movimentacao);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
            var success = await _service.UpdateAsync(id, movimentacao);
            if (!success) return NotFound();
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
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}