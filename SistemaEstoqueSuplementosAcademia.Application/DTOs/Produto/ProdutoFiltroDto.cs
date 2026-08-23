// Application/DTOs/Produto/ProdutoFiltroDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto
{
    public class ProdutoFiltroDto
    {
        public string? Nome { get; set; }
        public int? CategoriaId { get; set; }
        public bool? Ativo { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
    }
}