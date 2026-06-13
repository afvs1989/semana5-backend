using Microsoft.EntityFrameworkCore;
using VehiculosApi.Domain.Entities;

namespace VehiculosApi.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Marca).HasMaxLength(50).IsRequired();
            entity.Property(v => v.Modelo).HasMaxLength(50).IsRequired();
            entity.Property(v => v.Color).HasMaxLength(30).IsRequired();
            entity.Property(v => v.Placa).HasMaxLength(10).IsRequired();
            entity.HasIndex(v => v.Placa).IsUnique();
            entity.Property(v => v.Vin).HasMaxLength(17).IsRequired();
            entity.HasIndex(v => v.Vin).IsUnique();
            entity.Property(v => v.TipoCombustible).HasMaxLength(20).IsRequired();
            entity.Property(v => v.Precio).HasPrecision(18, 2);
            entity.Property(v => v.Kilometraje).HasPrecision(18, 2);
            entity.Property(v => v.Estado).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.NombreUsuario).HasMaxLength(50).IsRequired();
            entity.HasIndex(u => u.NombreUsuario).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Rol).HasMaxLength(20).IsRequired();
        });
    }
}
