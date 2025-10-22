using Microsoft.EntityFrameworkCore;
using MottuApi.Models;
using System.Linq;

namespace MottuApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos => Set<Moto>();
        public DbSet<Patio> Patios => Set<Patio>();
        public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Moto>()
                .HasIndex(m => m.Placa)
                .IsUnique();

            modelBuilder.Entity<Patio>()
                .HasIndex(p => p.Nome)
                .IsUnique();

            modelBuilder.Entity<Patio>()
                .HasIndex(p => p.Localizacao)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var boolProps = entityType.ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(bool));

                foreach (var prop in boolProps)
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(prop.Name)
                        .HasConversion<int>();
                }
            }
        }
    }
}