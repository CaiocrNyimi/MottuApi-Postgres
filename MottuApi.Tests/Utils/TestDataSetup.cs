using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MottuApi.Tests.Utils
{
    public static class TestDataSetup
    {
        public static async Task<int> CriarPatioERetornarIdAsync(HttpClient client)
        {
            var body = new
            {
                nome = $"Pátio Teste {Guid.NewGuid():N}",
                localizacao = $"Rua Teste {Guid.NewGuid():N}"
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/patio", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(responseBody);
            return doc.RootElement.GetProperty("id").GetInt32();
        }

        public static async Task RemoverPatioAsync(HttpClient client, int id)
        {
            await client.DeleteAsync($"/api/v1/patio/{id}");
        }

        public static async Task<int> CriarMotoSemPatioERetornarIdAsync(HttpClient client)
        {
            var body = new
            {
                modelo = $"Honda CG 160 {Guid.NewGuid():N}",
                placa = $"ABC{new Random().Next(1, 9)}D{new Random().Next(10, 99)}",
                status = "Disponível",
                patioId = (int?)null,
                dataEntrada = (string?)null
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/v1/moto", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(responseBody);
            return doc.RootElement.GetProperty("id").GetInt32();
        }

        public static async Task RemoverMotoAsync(HttpClient client, int id)
        {
            await client.DeleteAsync($"/api/v1/moto/{id}");
        }
    }
}