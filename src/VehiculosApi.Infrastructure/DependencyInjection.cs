using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehiculosApi.Application.Interfaces;
using VehiculosApi.Application.Services;
using VehiculosApi.Domain.Entities;
using VehiculosApi.Infrastructure.Data;
using VehiculosApi.Infrastructure.Repositories;
using VehiculosApi.Infrastructure.Services;

namespace VehiculosApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IVehiculoRepository, VehiculoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IVehiculoService, VehiculoService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

    public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        if (!await context.Usuarios.AnyAsync())
        {
            context.Usuarios.Add(new Usuario
            {
                NombreUsuario = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Rol = "Administrador"
            });
            await context.SaveChangesAsync();
        }

        if (!await context.Vehiculos.AnyAsync())
        {
            context.Vehiculos.AddRange(
                new Vehiculo
                {
                    Marca = "Toyota", Modelo = "Corolla", Anio = 2022, Color = "Blanco",
                    Placa = "ABC123", Vin = "1HGBH41JXMN109186", Kilometraje = 15000,
                    TipoCombustible = "Gasolina", Precio = 85000000, Estado = "Disponible"
                },
                new Vehiculo
                {
                    Marca = "Mazda", Modelo = "CX-5", Anio = 2023, Color = "Rojo",
                    Placa = "XYZ789", Vin = "JM3KFBDM5K0123456", Kilometraje = 8000,
                    TipoCombustible = "Gasolina", Precio = 120000000, Estado = "Disponible"
                });
            await context.SaveChangesAsync();
        }
    }
}
