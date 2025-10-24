using Xunit;
using Microsoft.Extensions.Configuration;
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
                .UseInMemoryDatabase(databaseName: "AuthDb")
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string?>("Jwt:Key", "chavefake123456789012345678901234567890"),
                    new KeyValuePair<string, string?>("Jwt:Issuer", "MottuApi")
                })
                .Build();

            return new AuthService(context, config);
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