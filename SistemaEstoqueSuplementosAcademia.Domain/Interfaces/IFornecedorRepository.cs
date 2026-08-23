// Domain/Interfaces/IFornecedorRepository.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IFornecedorRepository
    {
        Task<Fornecedor?> ObterPorIdAsync(int id);
        Task<(IEnumerable<Fornecedor> Itens, int TotalRegistros)> ObterPaginadoAsync(
            string? nome, bool? ativo, int pagina, int tamanhoPagina);
        Task<bool> ExisteComCnpjAsync(string cnpj, int? idParaIgnorar = null);
        Task AdicionarAsync(Fornecedor fornecedor);
        void Atualizar(Fornecedor fornecedor);
        Task SalvarAsync();
    }
}