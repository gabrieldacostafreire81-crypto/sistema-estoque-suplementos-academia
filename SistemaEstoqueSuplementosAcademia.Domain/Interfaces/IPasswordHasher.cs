// Domain/Interfaces/IPasswordHasher.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string GerarHash(string senha);
        bool VerificarSenha(string senha, string hash);
    }
}
