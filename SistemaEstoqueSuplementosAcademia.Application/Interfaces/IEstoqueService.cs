// Application/Interfaces/IEstoqueService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IEstoqueService
    {
        Task<MovimentacaoResponseDto> RegistrarEntradaAsync(MovimentacaoEntradaSaidaDto dto);
        Task<MovimentacaoResponseDto> RegistrarSaidaAsync(MovimentacaoEntradaSaidaDto dto);
        Task<MovimentacaoResponseDto> RegistrarAjusteAsync(MovimentacaoAjusteDto dto);
        Task<IEnumerable<MovimentacaoResponseDto>> ObterHistoricoAsync(int? produtoId, DateTime? dataInicial, DateTime? dataFinal);
    }
}
