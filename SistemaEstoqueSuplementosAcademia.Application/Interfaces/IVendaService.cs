// Application/Interfaces/IVendaService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda;

namespace SistemaEstoqueSuplementosAcademia.Application.Interfaces
{
    public interface IVendaService
    {
        Task<VendaResponseDto> CriarAsync(VendaCreateDto dto, int usuarioId);
        Task<VendaResponseDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<VendaResponseDto>> ObterTodasAsync();
    }
}