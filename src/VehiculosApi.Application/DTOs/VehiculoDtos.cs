namespace VehiculosApi.Application.DTOs;

public record VehiculoDto(
    int Id,
    string Marca,
    string Modelo,
    int Anio,
    string Color,
    string Placa,
    string Vin,
    decimal Kilometraje,
    string TipoCombustible,
    decimal Precio,
    string Estado,
    DateTime FechaRegistro);

public record CrearVehiculoRequest(
    string Marca,
    string Modelo,
    int Anio,
    string Color,
    string Placa,
    string Vin,
    decimal Kilometraje,
    string TipoCombustible,
    decimal Precio,
    string Estado);

public record ActualizarVehiculoRequest(
    string Marca,
    string Modelo,
    int Anio,
    string Color,
    string Placa,
    string Vin,
    decimal Kilometraje,
    string TipoCombustible,
    decimal Precio,
    string Estado);
