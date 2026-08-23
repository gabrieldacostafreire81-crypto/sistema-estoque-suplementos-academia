// Application/DTOs/Common/PagedResultDto.cs
namespace SistemaEstoqueSuplementosAcademia.Application.DTOs.Common
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Itens { get; set; } = Enumerable.Empty<T>();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas => (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);
    }
}