using SistemaEstoqueSuplementosAcademia.Application.DTOs.Categoria;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaResponseDto> CriarAsync(CategoriaCreateDto dto);
        Task<CategoriaResponseDto> AtualizarAsync(int id, CategoriaUpdateDto dto);
        Task<CategoriaResponseDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<CategoriaResponseDto>> ObterTodasAsync();
        Task InativarAsync(int id);
    }
}