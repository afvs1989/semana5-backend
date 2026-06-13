using VehiculosApi.Application.DTOs;

namespace VehiculosApi.Application.Interfaces;

public interface IVehiculoService
{
    Task<IReadOnlyList<VehiculoDto>> ObtenerTodosAsync();
    Task<VehiculoDto?> ObtenerPorIdAsync(int id);
    Task<(VehiculoDto? Vehiculo, string? Error)> CrearAsync(CrearVehiculoRequest request);
    Task<(VehiculoDto? Vehiculo, string? Error)> ActualizarAsync(int id, ActualizarVehiculoRequest request);
    Task<(bool Success, string? Error)> EliminarAsync(int id);
}
