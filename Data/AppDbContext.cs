using ApiRelogios.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiRelogios.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Relogio> Relogios => Set<Relogio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // SQLite não tem tipo decimal nativo; guarda como REAL para permitir ordenar e comparar
        modelBuilder.Entity<Relogio>().Property(relogio => relogio.Preco).HasConversion<double>();

        modelBuilder.Entity<Relogio>().HasData(
            new Relogio { Id = 1, Marca = "Casio", Modelo = "F-91W", Tipo = "Digital", Preco = 149.90m },
            new Relogio { Id = 2, Marca = "Seiko", Modelo = "Presage SRPB43", Tipo = "Automático", Preco = 3499.00m },
            new Relogio { Id = 3, Marca = "Apple", Modelo = "Watch Series 10", Tipo = "Smartwatch", Preco = 4299.00m }
        );
    }
}
