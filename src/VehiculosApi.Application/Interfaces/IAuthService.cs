using VehiculosApi.Application.DTOs;

namespace VehiculosApi.Application.Interfaces;

public interface IAuthService
{
    Task<(bool Success, LoginResponse? Response, string? Error)> LoginAsync(LoginRequest request);
    Task LogoutAsync();
    Task<AuthStatusResponse> GetStatusAsync();
}
