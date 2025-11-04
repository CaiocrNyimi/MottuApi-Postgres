using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MottuApi.Tests.Utils;
using MottuApi.Data;

namespace MottuApi.Tests.Integration
{
    public class MotoCrudTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _db;

        public MotoCrudTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            var scope = factory.Services.CreateScope();
            _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
        public async Task CrudMoto_DeveExecutarComSucesso()
        {
            var patioId = await TestDataSetup.CriarPatioERetornarIdAsync(_db);

            var postPayload = new
            {
                modelo = $"CG 160 {Guid.NewGuid():N}",
                placa = $"ABC{new Random().Next(1, 9)}D{new Random().Next(10, 99)}",
                status = "Disponível",
                patioId,
                dataEntrada = "2025-10-23T10:00:00"
            };

            var postContent = new StringContent(JsonSerializer.Serialize(postPayload), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/moto", postContent);
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

            var postBody = await postResponse.Content.ReadAsStringAsync();
            var motoId = JsonDocument.Parse(postBody).RootElement.GetProperty("id").GetInt32();

            var getResponse = await _client.GetAsync($"/api/v1/moto/{motoId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var putPayload = new
            {
                modelo = $"Atualizada {Guid.NewGuid():N}",
                placa = $"XYZ{new Random().Next(1, 9)}D{new Random().Next(10, 99)}",
                status = "Manutenção",
                patioId,
                dataEntrada = "2025-10-24T10:00:00"
            };

            var putContent = new StringContent(JsonSerializer.Serialize(putPayload), Encoding.UTF8, "application/json");
            var putResponse = await _client.PutAsync($"/api/v1/moto/{motoId}", putContent);
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

            var deleteResponse = await _client.DeleteAsync($"/api/v1/moto/{motoId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            await TestDataSetup.RemoverPatioAsync(_db, patioId);
        }
    }
}