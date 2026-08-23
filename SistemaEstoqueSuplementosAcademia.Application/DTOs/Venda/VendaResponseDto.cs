// Application/DTOs/Venda/VendaResponseDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda
{
    public class VendaResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public decimal ValorTotal { get; set; }
        public string? FormaPagamento { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<VendaItemResponseDto> Itens { get; set; } = new();
    }
}