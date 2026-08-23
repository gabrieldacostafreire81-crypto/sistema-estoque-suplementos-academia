// Domain/Interfaces/IMarcaRepository.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IMarcaRepository
    {
        Task<Marca?> ObterPorIdAsync(int id);
        Task<IEnumerable<Marca>> ObterTodasAsync();
        Task<bool> ExisteComNomeAsync(string nome, int? idParaIgnorar = null);
        Task AdicionarAsync(Marca marca);
        void Atualizar(Marca marca);
        Task SalvarAsync();
    }
}