// Application/DTOs/Produto/ProdutoCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto
{
    public class ProdutoCreateDto
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required] public int CategoriaId { get; set; }
        [Required] public int MarcaId { get; set; }
        [Required] public int FornecedorId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de compra deve ser maior que zero.")]
        public decimal PrecoCompra { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de venda deve ser maior que zero.")]
        public decimal PrecoVenda { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O estoque mínimo não pode ser negativo.")]
        public int EstoqueMinimo { get; set; }
    }
}