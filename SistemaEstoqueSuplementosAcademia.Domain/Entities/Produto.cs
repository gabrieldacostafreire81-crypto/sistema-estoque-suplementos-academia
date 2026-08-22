// Domain/Entities/Produto.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public int MarcaId { get; set; }
        public Marca Marca { get; set; } = null!;

        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; } = null!;

        public decimal PrecoCompra { get; set; }
        public decimal PrecoVenda { get; set; }
        public int EstoqueAtual { get; set; }
        public int EstoqueMinimo { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
