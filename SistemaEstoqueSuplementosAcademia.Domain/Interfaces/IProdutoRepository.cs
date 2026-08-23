// Domain/Interfaces/IProdutoRepository.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> ObterPorIdAsync(int id);
        Task<(IEnumerable<Produto> Itens, int TotalRegistros)> ObterPaginadoAsync(
            string? nome, int? categoriaId, bool? ativo, int pagina, int tamanhoPagina);
        Task AdicionarAsync(Produto produto);
        void Atualizar(Produto produto);
        Task SalvarAsync();
    }
}