using Microsoft.AspNetCore.Mvc;
using MottuApi.Examples;
using MottuApi.Dtos;
using MottuApi.Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace MottuApi.Controllers
{
    /// <summary>
    /// Controller responsável pela autenticação de usuários.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="dto">Dados do usuário para registro.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost("registrar")]
        [SwaggerRequestExample(typeof(RegisterRequestDto), typeof(RegisterRequestExample))]
        public async Task<IActionResult> Registrar([FromBody] RegisterRequestDto dto)
        {
            var sucesso = await _authService.RegistrarAsync(dto);
            return sucesso ? Ok("Usuário registrado com sucesso.") : BadRequest("Usuário já existe.");
        }

        /// <summary>
        /// Realiza login e retorna um token JWT.
        /// </summary>
        /// <param name="dto">Credenciais do usuário.</param>
        /// <returns>Token JWT ou erro de autenticação.</returns>
        [HttpPost("login")]
        [SwaggerRequestExample(typeof(LoginRequestDto), typeof(LoginRequestExample))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            return token == null ? Unauthorized("Credenciais inválidas.") : Ok(new { token });
        }
    }
}