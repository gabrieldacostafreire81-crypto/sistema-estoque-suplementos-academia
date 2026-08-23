// Application/DTOs/Estoque/MovimentacaoAjusteDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque
{
    public class MovimentacaoAjusteDto
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
        public int NovaQuantidade { get; set; }

        [Required(ErrorMessage = "O motivo do ajuste é obrigatório.")]
        [MinLength(10, ErrorMessage = "Descreva o motivo com pelo menos 10 caracteres.")]
        public string Motivo { get; set; } = string.Empty;
    }
}
