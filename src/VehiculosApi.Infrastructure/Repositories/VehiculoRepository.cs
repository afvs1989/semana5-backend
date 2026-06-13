using Microsoft.EntityFrameworkCore;
using VehiculosApi.Application.Interfaces;
using VehiculosApi.Domain.Entities;
using VehiculosApi.Infrastructure.Data;

namespace VehiculosApi.Infrastructure.Repositories;

public class VehiculoRepository(AppDbContext context) : IVehiculoRepository
{
    public async Task<IReadOnlyList<Vehiculo>> ObtenerTodosAsync() =>
        await context.Vehiculos.OrderByDescending(v => v.FechaRegistro).ToListAsync();

    public async Task<Vehiculo?> ObtenerPorIdAsync(int id) =>
        await context.Vehiculos.FindAsync(id);

    public async Task<bool> ExistePlacaAsync(string placa, int? excluirId = null) =>
        await context.Vehiculos.AnyAsync(v =>
            v.Placa == placa.Trim().ToUpperInvariant() && (!excluirId.HasValue || v.Id != excluirId));

    public async Task<bool> ExisteVinAsync(string vin, int? excluirId = null) =>
        await context.Vehiculos.AnyAsync(v =>
            v.Vin == vin.Trim().ToUpperInvariant() && (!excluirId.HasValue || v.Id != excluirId));

    public async Task<Vehiculo> CrearAsync(Vehiculo vehiculo)
    {
        context.Vehiculos.Add(vehiculo);
        await context.SaveChangesAsync();
        return vehiculo;
    }

    public async Task ActualizarAsync(Vehiculo vehiculo)
    {
        context.Vehiculos.Update(vehiculo);
        await context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Vehiculo vehiculo)
    {
        context.Vehiculos.Remove(vehiculo);
        await context.SaveChangesAsync();
    }
}
