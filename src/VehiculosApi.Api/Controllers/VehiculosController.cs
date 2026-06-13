using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiculosApi.Application.DTOs;
using VehiculosApi.Application.Interfaces;

namespace VehiculosApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiculosController(IVehiculoService vehiculoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var vehiculos = await vehiculoService.ObtenerTodosAsync();
        return Ok(vehiculos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var vehiculo = await vehiculoService.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            return NotFound(new { message = "Vehículo no encontrado." });

        return Ok(vehiculo);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] CrearVehiculoRequest request)
    {
        var (vehiculo, error) = await vehiculoService.CrearAsync(request);
        if (error is not null)
            return BadRequest(new { message = error });

        return CreatedAtAction(nameof(ObtenerPorId), new { id = vehiculo!.Id }, vehiculo);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarVehiculoRequest request)
    {
        var (vehiculo, error) = await vehiculoService.ActualizarAsync(id, request);
        if (error == "Vehículo no encontrado.")
            return NotFound(new { message = error });
        if (error is not null)
            return BadRequest(new { message = error });

        return Ok(vehiculo);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var (success, error) = await vehiculoService.EliminarAsync(id);
        if (!success)
            return NotFound(new { message = error });

        return Ok(new { message = "Vehículo eliminado correctamente." });
    }
}
