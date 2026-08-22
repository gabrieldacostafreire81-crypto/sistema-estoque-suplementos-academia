// Domain/Entities/Marca.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Entities
{
    public class Marca
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;

        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
