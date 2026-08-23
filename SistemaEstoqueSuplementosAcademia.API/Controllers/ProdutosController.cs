// Controllers/ProdutosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _service;

        public ProdutosController(IProdutoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPaginado([FromQuery] ProdutoFiltroDto filtro)
        {
            var resultado = await _service.ObterPaginadoAsync(filtro);
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> ObterPorId(int id)
        {
            var produto = await _service.ObterPorIdAsync(id);
            if (produto is null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDto>> Criar(ProdutoCreateDto dto)
        {
            try
            {
                var produto = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> Atualizar(int id, ProdutoUpdateDto dto)
        {
            try
            {
                return Ok(await _service.AtualizarAsync(id, dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { mensagem = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Inativar(int id)
        {
            try
            {
                await _service.InativarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }
    }
}