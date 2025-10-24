using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MottuApi.Tests.Integration
{
    public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Registrar_DeveRetornar200_QuandoUsuarioNovo()
        {
            var body = new
            {
                username = $"usuarioNovo_{Guid.NewGuid():N}",
                senha = "senhaSegura"
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/auth/registrar", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_DeveRetornarToken_QuandoCredenciaisValidas()
        {
            var username = $"usuarioLogin_{Guid.NewGuid():N}";
            var senha = "senhaSegura";

            // Registrar usuário
            var registro = new { username, senha };
            var regContent = new StringContent(JsonSerializer.Serialize(registro), Encoding.UTF8, "application/json");
            var regResponse = await _client.PostAsync("/api/v1/auth/registrar", regContent);
            Assert.Equal(HttpStatusCode.OK, regResponse.StatusCode);

            // Login
            var login = new { username, senha };
            var loginContent = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/v1/auth/login", loginContent);
            var json = await loginResponse.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
            Assert.Contains("token", json);
        }

        [Fact]
        public async Task Login_DeveFalhar_QuandoSenhaIncorreta()
        {
            var username = $"usuarioErro_{Guid.NewGuid():N}";
            var senhaCorreta = "senhaSegura";
            var senhaErrada = "senhaErrada";

            var registro = new { username, senha = senhaCorreta };
            var regContent = new StringContent(JsonSerializer.Serialize(registro), Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/v1/auth/registrar", regContent);

            var login = new { username, senha = senhaErrada };
            var loginContent = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/auth/login", loginContent);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}