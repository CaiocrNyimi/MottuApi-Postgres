using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using MottuApi.Tests.Utils;

namespace MottuApi.Tests.Integration
{
    public class PatioCrudTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PatioCrudTests(WebApplicationFactory<Program> factory)
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
        public async Task CrudPatio_DeveExecutarComSucesso()
        {
            var postPayload = new
            {
                nome = $"Pátio {Guid.NewGuid():N}",
                localizacao = $"Rua {Guid.NewGuid():N}"
            };

            var postContent = new StringContent(JsonSerializer.Serialize(postPayload), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/patio", postContent);
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            var postBody = await postResponse.Content.ReadAsStringAsync();
            var patioId = JsonDocument.Parse(postBody).RootElement.GetProperty("id").GetInt32();

            var getResponse = await _client.GetAsync($"/api/v1/patio/{patioId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var putPayload = new
            {
                nome = $"Atualizado {Guid.NewGuid():N}",
                localizacao = $"Nova Rua {Guid.NewGuid():N}"
            };

            var putContent = new StringContent(JsonSerializer.Serialize(putPayload), Encoding.UTF8, "application/json");
            var putResponse = await _client.PutAsync($"/api/v1/patio/{patioId}", putContent);
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

            var deleteResponse = await _client.DeleteAsync($"/api/v1/patio/{patioId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}