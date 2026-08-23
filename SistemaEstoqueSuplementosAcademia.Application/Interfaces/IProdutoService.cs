// Application/Interfaces/IProdutoService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Common;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoResponseDto> CriarAsync(ProdutoCreateDto dto);
        Task<ProdutoResponseDto> AtualizarAsync(int id, ProdutoUpdateDto dto);
        Task<ProdutoResponseDto?> ObterPorIdAsync(int id);
        Task<PagedResultDto<ProdutoResponseDto>> ObterPaginadoAsync(ProdutoFiltroDto filtro);
        Task InativarAsync(int id);
    }
}
