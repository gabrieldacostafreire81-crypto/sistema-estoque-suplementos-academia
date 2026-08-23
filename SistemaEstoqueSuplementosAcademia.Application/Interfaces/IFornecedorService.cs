// Application/Interfaces/IFornecedorService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Fornecedor;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IFornecedorService
    {
        Task<FornecedorResponseDto> CriarAsync(FornecedorCreateDto dto);
        Task<FornecedorResponseDto> AtualizarAsync(int id, FornecedorUpdateDto dto);
        Task<FornecedorResponseDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<FornecedorResponseDto>> ObterTodosAsync();
        Task InativarAsync(int id);
    }
}