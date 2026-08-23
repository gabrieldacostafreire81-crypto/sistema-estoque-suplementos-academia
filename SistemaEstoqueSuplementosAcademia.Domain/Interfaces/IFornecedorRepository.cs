// Domain/Interfaces/IFornecedorRepository.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IFornecedorRepository
    {
        Task<Fornecedor?> ObterPorIdAsync(int id);
        Task<IEnumerable<Fornecedor>> ObterTodosAsync();
        Task<bool> ExisteComCnpjAsync(string cnpj, int? idParaIgnorar = null);
        Task AdicionarAsync(Fornecedor fornecedor);
        void Atualizar(Fornecedor fornecedor);
        Task SalvarAsync();
    }
}