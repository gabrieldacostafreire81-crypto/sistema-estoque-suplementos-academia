// Application/Interfaces/IVendaService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda;
using SistemaEstoqueSuplementosAcademia.Domain.Models;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IVendaService
    {
        Task<VendaResponseDto> CriarAsync(VendaCreateDto dto, int usuarioId);
        Task<VendaResponseDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<VendaResponseDto>> ObterTodasAsync();
        Task<IEnumerable<VendaResponseDto>> ObterPorPeriodoAsync(DateTime? dataInicial, DateTime? dataFinal);
        Task<IEnumerable<ProdutoMaisVendido>> ObterMaisVendidosAsync(int topN, DateTime? dataInicial, DateTime? dataFinal);
    }
}