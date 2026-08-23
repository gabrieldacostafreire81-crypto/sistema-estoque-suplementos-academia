// Domain/Interfaces/IVendaRepository.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IVendaRepository
    {
        Task AdicionarAsync(Venda venda);
        Task<Venda?> ObterPorIdAsync(int id);
        Task<IEnumerable<Venda>> ObterTodasAsync();
    }
}