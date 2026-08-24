// Domain/Models/ProdutoMaisVendido.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Models
{
    public class ProdutoMaisVendido
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public int QuantidadeTotalVendida { get; set; }
        public decimal FaturamentoTotal { get; set; }
    }
}