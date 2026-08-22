// Domain/Entities/MovimentacaoEstoque.cs
using SistemaEstoqueSuplementosAcademia.Domain.Enums;

namespace SistemaEstoqueSuplementosAcademia.Domain.Entities
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public TipoMovimentacaoEstoque Tipo { get; set; }
        public int Quantidade { get; set; }
        public string? Motivo { get; set; }
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
    }
}