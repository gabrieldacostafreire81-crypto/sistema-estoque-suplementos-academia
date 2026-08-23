// Application/DTOs/Estoque/MovimentacaoResponseDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque
{
    public class MovimentacaoResponseDto
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public string? Motivo { get; set; }
        public DateTime DataHora { get; set; }
        public int EstoqueResultante { get; set; }
    }
}