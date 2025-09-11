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
    }
}