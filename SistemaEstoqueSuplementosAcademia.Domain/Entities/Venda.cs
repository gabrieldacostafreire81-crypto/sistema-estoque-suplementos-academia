// Domain/Entities/Venda.cs
namespace SistemaEstoqueSuplementosAcademia.Domain.Entities
{
    public class Venda
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public decimal ValorTotal { get; set; }
        public string? FormaPagamento { get; set; }
        public string Status { get; set; } = "Concluida";

        public ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
}