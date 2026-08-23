// Application/Interfaces/IMarcaService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Marca;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IMarcaService
    {
        Task<MarcaResponseDto> CriarAsync(MarcaCreateDto dto);
        Task<MarcaResponseDto> AtualizarAsync(int id, MarcaUpdateDto dto);
        Task<MarcaResponseDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<MarcaResponseDto>> ObterTodasAsync();
        Task InativarAsync(int id);
    }
}
