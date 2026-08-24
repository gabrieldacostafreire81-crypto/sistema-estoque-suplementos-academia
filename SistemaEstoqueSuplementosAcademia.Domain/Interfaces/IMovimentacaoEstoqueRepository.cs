using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        Task AdicionarAsync(MovimentacaoEstoque movimentacao);
        Task<IEnumerable<MovimentacaoEstoque>> ObterPorFiltroAsync(
            int? produtoId, DateTime? dataInicial, DateTime? dataFinal);
    }
}