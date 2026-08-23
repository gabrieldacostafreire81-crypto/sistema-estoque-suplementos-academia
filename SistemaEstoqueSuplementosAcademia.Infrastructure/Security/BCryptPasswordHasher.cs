// Infrastructure/Security/BCryptPasswordHasher.cs
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string GerarHash(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha, workFactor: 11);
        }

        public bool VerificarSenha(string senha, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
    }
}
