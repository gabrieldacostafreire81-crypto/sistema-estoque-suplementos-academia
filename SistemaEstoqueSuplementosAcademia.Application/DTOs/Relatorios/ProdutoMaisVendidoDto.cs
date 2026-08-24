// Application/DTOs/Relatorios/ProdutoMaisVendidoDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Relatorios
{
    public class ProdutoMaisVendidoDto
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public int QuantidadeTotalVendida { get; set; }
        public decimal FaturamentoTotal { get; set; }
    }
}