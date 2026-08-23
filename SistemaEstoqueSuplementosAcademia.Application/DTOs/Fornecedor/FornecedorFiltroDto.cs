// Application/DTOs/Fornecedor/FornecedorFiltroDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Fornecedor
{
    public class FornecedorFiltroDto
    {
        public string? Nome { get; set; }
        public bool? Ativo { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
    }
}
