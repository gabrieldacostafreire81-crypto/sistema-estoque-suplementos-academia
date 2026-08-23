// Application/Interfaces/IFornecedorService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Common;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Fornecedor;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IFornecedorService
    {
        Task<FornecedorResponseDto> CriarAsync(FornecedorCreateDto dto);
        Task<FornecedorResponseDto> AtualizarAsync(int id, FornecedorUpdateDto dto);
        Task<FornecedorResponseDto?> ObterPorIdAsync(int id);
        Task<PagedResultDto<FornecedorResponseDto>> ObterPaginadoAsync(FornecedorFiltroDto filtro);
        Task InativarAsync(int id);
    }
}
