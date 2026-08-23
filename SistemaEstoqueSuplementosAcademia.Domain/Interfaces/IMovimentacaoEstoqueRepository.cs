// Domain/Interfaces/IMovimentacaoEstoqueRepository.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        Task AdicionarAsync(MovimentacaoEstoque movimentacao);
        Task<IEnumerable<MovimentacaoEstoque>> ObterPorProdutoAsync(int produtoId);
    }
}