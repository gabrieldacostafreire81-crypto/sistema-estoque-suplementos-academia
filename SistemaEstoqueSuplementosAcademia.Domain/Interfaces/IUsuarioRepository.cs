// Domain/Interfaces/IUsuarioRepository.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<bool> ExisteComEmailAsync(string email);
        Task AdicionarAsync(Usuario usuario);
    }
}
