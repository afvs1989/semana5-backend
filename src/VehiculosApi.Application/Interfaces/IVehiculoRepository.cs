using VehiculosApi.Domain.Entities;

namespace VehiculosApi.Application.Interfaces;

public interface IVehiculoRepository
{
    Task<IReadOnlyList<Vehiculo>> ObtenerTodosAsync();
    Task<Vehiculo?> ObtenerPorIdAsync(int id);
    Task<bool> ExistePlacaAsync(string placa, int? excluirId = null);
    Task<bool> ExisteVinAsync(string vin, int? excluirId = null);
    Task<Vehiculo> CrearAsync(Vehiculo vehiculo);
    Task ActualizarAsync(Vehiculo vehiculo);
    Task EliminarAsync(Vehiculo vehiculo);
}
