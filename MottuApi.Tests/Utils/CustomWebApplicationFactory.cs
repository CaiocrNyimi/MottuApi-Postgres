using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MottuApi.Data;
using MottuApi.Tests.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MottuApi.Tests.Utils
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("JWT_KEY", "jwt-key-para-tests-1234567890987654321");
            Environment.SetEnvironmentVariable("JWT_ISSUER", "mottuapi");
            Environment.SetEnvironmentVariable("JWT_AUDIENCE", "mottuapi-users");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                TestDataSetup.SeedAsync(db).Wait();
            });
        }
    }
}