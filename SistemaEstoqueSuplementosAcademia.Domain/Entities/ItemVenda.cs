// Domain/Entities/ItemVenda.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Entities
{
    public class ItemVenda
    {
        public int Id { get; set; }

        public int VendaId { get; set; }
        public Venda Venda { get; set; } = null!;

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        public int Quantidade { get; set; }
        public decimal PrecoUnitarioNaVenda { get; set; }
        public decimal Subtotal { get; set; }
    }
}
