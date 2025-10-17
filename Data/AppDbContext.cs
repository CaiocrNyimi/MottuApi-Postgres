using Microsoft.EntityFrameworkCore;
using MottuApi.Models;

namespace MottuApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos => Set<Moto>();
        public DbSet<Patio> Patios => Set<Patio>();
        public DbSet<Movimentacao> Movimentacoes => Set<Movimentacao>();

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
        }
    }
}