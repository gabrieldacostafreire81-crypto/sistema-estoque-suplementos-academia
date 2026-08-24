using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Models;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IVendaRepository
    {
        Task AdicionarAsync(Venda venda);
        Task<Venda?> ObterPorIdAsync(int id);
        Task<IEnumerable<Venda>> ObterTodasAsync();
        Task<IEnumerable<Venda>> ObterPorPeriodoAsync(DateTime? dataInicial, DateTime? dataFinal);
        Task<IEnumerable<ProdutoMaisVendido>> ObterMaisVendidosAsync(int topN, DateTime? dataInicial, DateTime? dataFinal);
    }
}