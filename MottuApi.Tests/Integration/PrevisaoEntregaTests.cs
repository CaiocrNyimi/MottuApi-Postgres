using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MottuApi.Tests.Integration
{
    public class PrevisaoEntregaTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PrevisaoEntregaTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            CriarUsuarioAdminAsync().GetAwaiter().GetResult();
            var token = AutenticarAdminAsync().GetAwaiter().GetResult();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task CriarUsuarioAdminAsync()
        {
            var payload = new { username = "admin", senha = "12345" };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/auth/registrar", content);

            if (response.StatusCode == HttpStatusCode.OK) return;

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var body = await response.Content.ReadAsStringAsync();
                if (body.Contains("Usuário já existe", StringComparison.OrdinalIgnoreCase)) return;
            }

            throw new InvalidOperationException($"Falha ao registrar usuário admin. Status: {(int)response.StatusCode}, Body: {await response.Content.ReadAsStringAsync()}");
        }

        private async Task<string> AutenticarAdminAsync()
        {
            var loginPayload = new { username = "admin", senha = "12345" };
            var content = new StringContent(JsonSerializer.Serialize(loginPayload), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/auth/login", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty("token", out var tokenElement) &&
                tokenElement.ValueKind == JsonValueKind.String &&
                !string.IsNullOrWhiteSpace(tokenElement.GetString()))
                return tokenElement.GetString()!;

            throw new InvalidOperationException("Token inválido ou ausente na resposta.");
        }

        [Fact]
        public async Task PreverTempoEntrega_DeveRetornar200()
        {
            var body = new
            {
                modelo = $"Biz {Guid.NewGuid():N}",
                distanciaKm = 10
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/entrega/prever-tempo", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("tempoEstimadoMin", responseBody);
        }

        [Fact]
        public async Task PreverTempoEntrega_DeveFalhar_QuandoModeloAusente()
        {
            var body = new
            {
                modelo = "",
                distanciaKm = 10
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/entrega/prever-tempo", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}