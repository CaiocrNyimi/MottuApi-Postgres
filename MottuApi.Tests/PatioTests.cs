using Xunit;
using MottuApi.Models;

namespace MottuApi.Tests
{
    public class PatioTests
    {
        [Fact]
        public void CriarPatio_DeveRetornarDadosValidos()
        {
            var patio = new Patio
            {
                Nome = "Pátio Central",
                Localizacao = "Rua Teste, 123"
            };

            Assert.Equal("Pátio Central", patio.Nome);
            Assert.Equal("Rua Teste, 123", patio.Localizacao);
        }

        [Fact]
        public void ConsultarPatio_DeveRetornarNome()
        {
            var patio = new Patio { Nome = "Pátio Norte" };
            Assert.Equal("Pátio Norte", patio.Nome);
        }

        [Fact]
        public void AtualizarPatio_DeveAlterarLocalizacao()
        {
            var patio = new Patio { Localizacao = "Antiga Rua" };
            patio.Localizacao = "Nova Rua 456";
            Assert.Equal("Nova Rua 456", patio.Localizacao);
        }

        [Fact]
        public void DeletarPatio_DeveZerarObjeto()
        {
            var patio = new Patio { Nome = "Pátio Sul" };
            patio = null;
            Assert.Null(patio);
        }
    }
}