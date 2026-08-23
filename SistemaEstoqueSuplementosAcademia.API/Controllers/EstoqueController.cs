// Controllers/EstoqueController.cs
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueService _service;

        public EstoqueController(IEstoqueService service)
        {
            _service = service;
        }

        [HttpPost("entrada")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarEntrada(MovimentacaoEntradaSaidaDto dto)
        {
            try
            {
                return Ok(await _service.RegistrarEntradaAsync(dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpPost("saida")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarSaida(MovimentacaoEntradaSaidaDto dto)
        {
            try
            {
                return Ok(await _service.RegistrarSaidaAsync(dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { mensagem = ex.Message }); }
        }

        [HttpPost("ajuste")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarAjuste(MovimentacaoAjusteDto dto)
        {
            try
            {
                return Ok(await _service.RegistrarAjusteAsync(dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpGet("produto/{produtoId}/historico")]
        public async Task<ActionResult<IEnumerable<MovimentacaoResponseDto>>> ObterHistorico(int produtoId)
        {
            return Ok(await _service.ObterHistoricoPorProdutoAsync(produtoId));
        }
    }
}
