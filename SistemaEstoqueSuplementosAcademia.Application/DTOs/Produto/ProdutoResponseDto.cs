// Application/DTOs/Produto/ProdutoResponseDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto
{
    public class ProdutoResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;

        public int MarcaId { get; set; }
        public string MarcaNome { get; set; } = string.Empty;

        public int FornecedorId { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;

        public decimal PrecoCompra { get; set; }
        public decimal PrecoVenda { get; set; }
        public int EstoqueAtual { get; set; }
        public int EstoqueMinimo { get; set; }
        public bool EstoqueBaixo { get; set; }
        public bool Ativo { get; set; }
    }
}