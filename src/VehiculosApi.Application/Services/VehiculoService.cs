using VehiculosApi.Application.DTOs;
using VehiculosApi.Application.Interfaces;
using VehiculosApi.Domain.Entities;

namespace VehiculosApi.Application.Services;

public class VehiculoService(IVehiculoRepository repository) : IVehiculoService
{
    private static readonly HashSet<string> EstadosValidos =
        ["Disponible", "Vendido", "En mantenimiento", "Reservado"];

    private static readonly HashSet<string> CombustiblesValidos =
        ["Gasolina", "Diésel", "Eléctrico", "Híbrido", "GLP"];

    public async Task<IReadOnlyList<VehiculoDto>> ObtenerTodosAsync()
    {
        var vehiculos = await repository.ObtenerTodosAsync();
        return vehiculos.Select(MapToDto).ToList();
    }

    public async Task<VehiculoDto?> ObtenerPorIdAsync(int id)
    {
        var vehiculo = await repository.ObtenerPorIdAsync(id);
        return vehiculo is null ? null : MapToDto(vehiculo);
    }

    public async Task<(VehiculoDto? Vehiculo, string? Error)> CrearAsync(CrearVehiculoRequest request)
    {
        var error = Validar(request.Marca, request.Modelo, request.Anio, request.Color, request.Placa,
            request.Vin, request.Kilometraje, request.TipoCombustible, request.Precio, request.Estado);
        if (error is not null)
            return (null, error);

        if (await repository.ExistePlacaAsync(request.Placa))
            return (null, "Ya existe un vehículo con esa placa.");

        if (await repository.ExisteVinAsync(request.Vin))
            return (null, "Ya existe un vehículo con ese VIN.");

        var vehiculo = new Vehiculo
        {
            Marca = request.Marca.Trim(),
            Modelo = request.Modelo.Trim(),
            Anio = request.Anio,
            Color = request.Color.Trim(),
            Placa = request.Placa.Trim().ToUpperInvariant(),
            Vin = request.Vin.Trim().ToUpperInvariant(),
            Kilometraje = request.Kilometraje,
            TipoCombustible = request.TipoCombustible.Trim(),
            Precio = request.Precio,
            Estado = request.Estado.Trim(),
            FechaRegistro = DateTime.UtcNow
        };

        var creado = await repository.CrearAsync(vehiculo);
        return (MapToDto(creado), null);
    }

    public async Task<(VehiculoDto? Vehiculo, string? Error)> ActualizarAsync(int id, ActualizarVehiculoRequest request)
    {
        var vehiculo = await repository.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            return (null, "Vehículo no encontrado.");

        var error = Validar(request.Marca, request.Modelo, request.Anio, request.Color, request.Placa,
            request.Vin, request.Kilometraje, request.TipoCombustible, request.Precio, request.Estado);
        if (error is not null)
            return (null, error);

        if (await repository.ExistePlacaAsync(request.Placa, id))
            return (null, "Ya existe otro vehículo con esa placa.");

        if (await repository.ExisteVinAsync(request.Vin, id))
            return (null, "Ya existe otro vehículo con ese VIN.");

        vehiculo.Marca = request.Marca.Trim();
        vehiculo.Modelo = request.Modelo.Trim();
        vehiculo.Anio = request.Anio;
        vehiculo.Color = request.Color.Trim();
        vehiculo.Placa = request.Placa.Trim().ToUpperInvariant();
        vehiculo.Vin = request.Vin.Trim().ToUpperInvariant();
        vehiculo.Kilometraje = request.Kilometraje;
        vehiculo.TipoCombustible = request.TipoCombustible.Trim();
        vehiculo.Precio = request.Precio;
        vehiculo.Estado = request.Estado.Trim();

        await repository.ActualizarAsync(vehiculo);
        return (MapToDto(vehiculo), null);
    }

    public async Task<(bool Success, string? Error)> EliminarAsync(int id)
    {
        var vehiculo = await repository.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            return (false, "Vehículo no encontrado.");

        await repository.EliminarAsync(vehiculo);
        return (true, null);
    }

    private static string? Validar(string marca, string modelo, int anio, string color, string placa,
        string vin, decimal kilometraje, string tipoCombustible, decimal precio, string estado)
    {
        if (string.IsNullOrWhiteSpace(marca))
            return "La marca es obligatoria.";
        if (string.IsNullOrWhiteSpace(modelo))
            return "El modelo es obligatorio.";
        if (anio < 1900 || anio > DateTime.UtcNow.Year + 1)
            return "El año no es válido.";
        if (string.IsNullOrWhiteSpace(color))
            return "El color es obligatorio.";
        if (string.IsNullOrWhiteSpace(placa) || placa.Trim().Length < 5)
            return "La placa debe tener al menos 5 caracteres.";
        if (string.IsNullOrWhiteSpace(vin) || vin.Trim().Length != 17)
            return "El VIN debe tener exactamente 17 caracteres.";
        if (kilometraje < 0)
            return "El kilometraje no puede ser negativo.";
        if (!CombustiblesValidos.Contains(tipoCombustible.Trim()))
            return "Tipo de combustible no válido.";
        if (precio <= 0)
            return "El precio debe ser mayor a cero.";
        if (!EstadosValidos.Contains(estado.Trim()))
            return "Estado no válido.";

        return null;
    }

    private static VehiculoDto MapToDto(Vehiculo v) => new(
        v.Id, v.Marca, v.Modelo, v.Anio, v.Color, v.Placa, v.Vin,
        v.Kilometraje, v.TipoCombustible, v.Precio, v.Estado, v.FechaRegistro);
}
