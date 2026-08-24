// Controllers/RelatoriosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RelatoriosController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public RelatoriosController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpGet("vendas-por-periodo")]
        public async Task<IActionResult> VendasPorPeriodo(
            [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal)
        {
            return Ok(await _vendaService.ObterPorPeriodoAsync(dataInicial, dataFinal));
        }

        [HttpGet("produtos-mais-vendidos")]
        public async Task<IActionResult> ProdutosMaisVendidos(
            [FromQuery] int topN = 10, [FromQuery] DateTime? dataInicial = null, [FromQuery] DateTime? dataFinal = null)
        {
            return Ok(await _vendaService.ObterMaisVendidosAsync(topN, dataInicial, dataFinal));
        }
    }
}