using MottuApi.Dtos;

namespace MottuApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegistrarAsync(RegisterRequestDto dto);
        Task<string?> LoginAsync(LoginRequestDto dto);
    }
}