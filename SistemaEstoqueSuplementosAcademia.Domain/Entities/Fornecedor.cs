// Domain/Entities/Fornecedor.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Entities
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
