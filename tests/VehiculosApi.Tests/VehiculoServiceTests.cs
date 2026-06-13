using Moq;
using VehiculosApi.Application.DTOs;
using VehiculosApi.Application.Interfaces;
using VehiculosApi.Application.Services;
using VehiculosApi.Domain.Entities;

namespace VehiculosApi.Tests;

public class VehiculoServiceTests
{
    private readonly Mock<IVehiculoRepository> _repositoryMock = new();
    private readonly VehiculoService _service;

    public VehiculoServiceTests()
    {
        _service = new VehiculoService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CrearAsync_RetornaError_CuandoVinEsInvalido()
    {
        var request = new CrearVehiculoRequest(
            "Toyota", "Corolla", 2022, "Blanco", "ABC1234", "VIN_CORTO",
            1000, "Gasolina", 50000000, "Disponible");

        var (vehiculo, error) = await _service.CrearAsync(request);

        Assert.Null(vehiculo);
        Assert.Equal("El VIN debe tener exactamente 17 caracteres.", error);
    }

    [Fact]
    public async Task CrearAsync_RetornaVehiculo_CuandoDatosSonValidos()
    {
        var request = new CrearVehiculoRequest(
            "Toyota", "Corolla", 2022, "Blanco", "ABC1234", "1HGBH41JXMN109186",
            1000, "Gasolina", 50000000, "Disponible");

        _repositoryMock.Setup(r => r.ExistePlacaAsync(It.IsAny<string>(), null)).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.ExisteVinAsync(It.IsAny<string>(), null)).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.CrearAsync(It.IsAny<Vehiculo>()))
            .ReturnsAsync((Vehiculo v) =>
            {
                v.Id = 1;
                return v;
            });

        var (vehiculo, error) = await _service.CrearAsync(request);

        Assert.Null(error);
        Assert.NotNull(vehiculo);
        Assert.Equal("Toyota", vehiculo!.Marca);
        Assert.Equal("ABC1234", vehiculo.Placa);
    }

    [Fact]
    public async Task EliminarAsync_RetornaError_CuandoNoExiste()
    {
        _repositoryMock.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Vehiculo?)null);

        var (success, error) = await _service.EliminarAsync(99);

        Assert.False(success);
        Assert.Equal("Vehículo no encontrado.", error);
    }
}
