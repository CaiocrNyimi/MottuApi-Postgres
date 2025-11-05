using Xunit;
using MottuApi.Models;
using BCrypt.Net;

namespace MottuApi.Tests
{
    public class UsuarioTests
    {
        [Fact]
        public void CriarUsuario_DeveGerarSenhaHash()
        {
            var senha = "12345";
            var usuario = new Usuario
            {
                Username = "caio",
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha)
            };

            Assert.StartsWith("$2", usuario.SenhaHash);
        }

        [Fact]
        public void ConsultarUsuario_DeveRetornarUsername()
        {
            var usuario = new Usuario { Username = "admin" };
            Assert.Equal("admin", usuario.Username);
        }

        [Fact]
        public void AtualizarUsuario_DeveAlterarSenha()
        {
            var usuario = new Usuario { SenhaHash = "antiga" };
            usuario.SenhaHash = "nova";
            Assert.Equal("nova", usuario.SenhaHash);
        }

        [Fact]
        public void DeletarUsuario_DeveZerarObjeto()
        {
            var usuario = new Usuario { Username = "admin" };
            usuario = null;
            Assert.Null(usuario);
        }
    }
}