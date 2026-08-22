using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> ObterPorIdAsync(int id);
        Task<IEnumerable<Categoria>> ObterTodasAsync();
        Task<bool> ExisteComNomeAsync(string nome, int? idParaIgnorar = null);
        Task AdicionarAsync(Categoria categoria);
        void Atualizar(Categoria categoria);
        Task SalvarAsync();
    }
}