// Application/DTOs/Estoque/MovimentacaoEntradaSaidaDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque
{
    public class MovimentacaoEntradaSaidaDto
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }
    }
}
