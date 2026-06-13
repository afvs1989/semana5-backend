using VehiculosApi.Domain.Entities;

namespace VehiculosApi.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario);
}
