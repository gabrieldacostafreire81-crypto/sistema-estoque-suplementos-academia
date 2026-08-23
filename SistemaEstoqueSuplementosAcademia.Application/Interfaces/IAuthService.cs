// Application/Interfaces/IAuthService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Auth;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<LoginResponseDto> CriarUsuarioAsync(UsuarioCreateDto dto);
    }
}