// Application/DTOs/Produto/ProdutoUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto
{
    public class ProdutoUpdateDto
    {
        [Required, MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required] public int CategoriaId { get; set; }
        [Required] public int MarcaId { get; set; }
        [Required] public int FornecedorId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal PrecoCompra { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal PrecoVenda { get; set; }

        [Range(0, int.MaxValue)]
        public int EstoqueMinimo { get; set; }
    }
}