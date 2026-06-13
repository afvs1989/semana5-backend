using Microsoft.EntityFrameworkCore;
using VehiculosApi.Application.Interfaces;
using VehiculosApi.Domain.Entities;
using VehiculosApi.Infrastructure.Data;

namespace VehiculosApi.Infrastructure.Repositories;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<Usuario?> ObtenerPorNombreAsync(string nombreUsuario) =>
        await context.Usuarios.FirstOrDefaultAsync(u =>
            u.NombreUsuario == nombreUsuario && u.Activo);
}
