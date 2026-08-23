// Application/DTOs/Venda/VendaItemResponseDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda
{
    public class VendaItemResponseDto
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitarioNaVenda { get; set; }
        public decimal Subtotal { get; set; }
    }
}