// Application/DTOs/Venda/VendaCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda
{
    public class VendaCreateDto
    {
        [Required(ErrorMessage = "A venda precisa ter pelo menos um item.")]
        [MinLength(1, ErrorMessage = "A venda precisa ter pelo menos um item.")]
        public List<VendaItemCreateDto> Itens { get; set; } = new();

        public string? FormaPagamento { get; set; }
    }
}