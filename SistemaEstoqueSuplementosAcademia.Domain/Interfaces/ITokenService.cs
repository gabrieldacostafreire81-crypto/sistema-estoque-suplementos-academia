// Domain/Interfaces/ITokenService.cs
using SistemaEstoqueSuplementosAcademia.Domain.Entities;

namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
