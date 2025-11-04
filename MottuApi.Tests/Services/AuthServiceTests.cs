using Xunit;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Services.Implementations;
using MottuApi.Dtos;

namespace MottuApi.Tests.Services
{
    public class AuthServiceTests
    {
        private AuthService CriarService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            return new AuthService(context);
        }

        [Fact]
        public async Task RegistrarAsync_DeveCriarUsuario()
        {
            var service = CriarService();
            var dto = new RegisterRequestDto
            {
                Username = "admin",
                Senha = "12345"
            };

            var result = await service.RegistrarAsync(dto);

            Assert.True(result);
        }

        [Fact]
        public async Task LoginAsync_DeveRetornarToken_QuandoCredenciaisValidas()
        {

            var service = CriarService();
            await service.RegistrarAsync(new RegisterRequestDto
            {
                Username = "admin",
                Senha = "12345"
            });

            var loginDto = new LoginRequestDto
            {
                Username = "admin",
                Senha = "12345"
            };

            var token = await service.LoginAsync(loginDto);

            Assert.NotNull(token);
            Assert.StartsWith("eyJ", token);
        }

        [Fact]
        public async Task LoginAsync_DeveRetornarNull_QuandoCredenciaisInvalidas()
        {
            var service = CriarService();
            await service.RegistrarAsync(new RegisterRequestDto
            {
                Username = "admin",
                Senha = "12345"
            });

            var loginDto = new LoginRequestDto
            {
                Username = "admin",
                Senha = "senhaErrada"
            };

            var token = await service.LoginAsync(loginDto);

            Assert.Null(token);
        }
    }
}